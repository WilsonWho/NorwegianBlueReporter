﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

namespace NorwegianBlue.IagoIntegration.Samples
{
    class IagoSample : ISampleAnalysis
    {
        // Example line:
        // INF [20140129-16:09:01.218] stats: {...}
        private static readonly Regex LineMatcher = new Regex(@"^INF \[(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})-(?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2})\.(?<millis>\d{3})\] stats: \{(?<stats>.*)\}$", RegexOptions.Compiled);
        private static readonly Regex DataMatcher = new Regex(@"(?<key>.+?):(?<value>.+?)[,$]", RegexOptions.Compiled);
        private static readonly Regex QuotteRemover = new Regex("([\"'])(?<key>.+?)\\1", RegexOptions.Compiled);
        private static readonly Regex VSlashRemover = new Regex("\\\\/", RegexOptions.Compiled);


        public DateTime TimeStamp { get; private set; }
        private readonly dynamic _analysisScratchPad = new ExpandoObject();

        private readonly Dictionary<string, Tuple<bool, double>> _stats = new Dictionary<string, Tuple<bool,double>>();
  
        private readonly Dictionary<string, string> _nonStats = new Dictionary<string, string>();
        private ReadOnlyDictionary<string, string> _roNonStats;

        private readonly List<AnalysisNote> _analysisNotes = new List<AnalysisNote>();
        private ReadOnlyCollection<AnalysisNote> _roAnalysisNotes;

        public ReadOnlyDictionary<string, string> NonStats
        {
            get { return _roNonStats ?? (_roNonStats = new ReadOnlyDictionary<string, string>(_nonStats)); }
        }

        public ReadOnlyCollection<AnalysisNote> AnalysisNotes
        {
            get { return _roAnalysisNotes ?? (_roAnalysisNotes = new ReadOnlyCollection<AnalysisNote>(_analysisNotes)); }
        }

        public dynamic AnalysisScratchPad { get { return _analysisScratchPad; } }

        public IagoSample()
        {
            Dictionary<object, object> configuration = YamlParser.GetConfiguration();
            if (configuration.ContainsKey("FieldsToIgnore"))
            {
                AnalysisScratchPad.FieldsToIgnore = configuration["FieldsToIgnore"];
            }
            else
            {
                AnalysisScratchPad.FieldsToIgnore = new List<string>();
            }
        }

        public void Parse(String input)
        {
            var matches = LineMatcher.Match(input);

            if (!matches.Success)
            {
                throw new InvalidDataException("input line can't be parsed by Iago parser: " + input);
            }


            TimeStamp = new DateTime(   Convert.ToInt32(matches.Groups["year"].Value),
                                        Convert.ToInt32(matches.Groups["month"].Value),
                                        Convert.ToInt32(matches.Groups["day"].Value),
                                        Convert.ToInt32(matches.Groups["hour"].Value),
                                        Convert.ToInt32(matches.Groups["minute"].Value),
                                        Convert.ToInt32(matches.Groups["second"].Value),
                                        Convert.ToInt32(matches.Groups["millis"].Value)
                                    );
            string data = matches.Groups["stats"].Value;

            foreach (Match kvpmatch in DataMatcher.Matches(data))
            {
                String key = kvpmatch.Groups["key"].Value;
                var temp = QuotteRemover.Match(key);
                key = temp.Groups["key"].Value;
                key = VSlashRemover.Replace(key, "/");

                String value = kvpmatch.Groups["value"].Value;

                if (_stats.ContainsKey(key) || _nonStats.ContainsKey(key))
                {
                    throw new InvalidDataException(
                        "input line has duplicate data intems and can't be parsed by Iago parser. Data name: " + key +
                        ", line: " + input);
                }

                var ignore = false;
                foreach (var ignoreRegex in AnalysisScratchPad.FieldsToIgnore)
                {
                    if (Regex.IsMatch(key, ignoreRegex))
                    {
                        ignore = true;
                        break;
                    }
                }

                double d;
                if (double.TryParse(value, out d))
                {
                    _stats[key] = new Tuple<bool, double>(ignore, d);
                }
                else
                {
                    _nonStats[key] = value;
                }
            }
        }

        public void Analyze(IEnumerable<StatAnalyzer> analyzers)
        {
            throw new NotImplementedException("No IagoStatistic analysis defined.");
        }

        public void AddAnalysisNote(AnalysisNote note)
        {
            _analysisNotes.Add(note);
        }

        private Dictionary<string, double> FilterStats(bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            //If there's a performance issue, cache the filtered dictionary, including the extra ignore list and see if this can be used in the next call.
            Dictionary<string, double> filteredStats;

            if (includeIgnored)
            {
                filteredStats = _stats.ToDictionary(stat => stat.Key, stat => stat.Value.Item2);
            }
            else
            {
                filteredStats = _stats.Where(stat => !stat.Value.Item1).ToDictionary(stat => stat.Key, stat => stat.Value.Item2);
            }

            if (null != extraIgnores)
            {
                foreach (var extraIgnore in extraIgnores)
                {
                    filteredStats.Remove(extraIgnore);
                }
            }

            return filteredStats;
        }

        public IEnumerator<KeyValuePair<string, double>> GetEnumerator(bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            return FilterStats(includeIgnored, extraIgnores).GetEnumerator();
        }

        public IEnumerator<KeyValuePair<string, double>> GetEnumerator()
        {
            return GetEnumerator(false, null);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get { return CountFiltered(false, null); } }

        public int CountFiltered(bool includeIgnored, List<string> extraIgnores)
        {
            return FilterStats(includeIgnored, extraIgnores).Count;
        }

        public bool ContainsKey(string key)
        {
            return ContainsKey(key, false, null);
        }

        public bool ContainsKey(string key, bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            return FilterStats(includeIgnored, extraIgnores).ContainsKey(key);
        }

        public bool TryGetValue(string key, out double value)
        {
            return TryGetValue(key, out value, false, null);
        }

        public bool TryGetValue(string key, out double value, bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            return FilterStats(includeIgnored, extraIgnores).TryGetValue(key, out value);
        }

        double IReadOnlyDictionary<string, double>.this[string key]
        {
            get { return this[key, false, null]; }
        }

        public double this[string key, bool includeIgnored, List<string> extraIgnores]
        {
            get { return FilterStats(includeIgnored, extraIgnores)[key]; }
        }

        public IEnumerable<string> Keys { get { return FilteredKeys(false, null); } }

        public IEnumerable<string> FilteredKeys(bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            return FilterStats(includeIgnored, extraIgnores).Keys;
        }


        public IEnumerable<double> Values { get { return FilteredValues(false, null); } }

        public IEnumerable<double> FilteredValues(bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            return FilterStats(includeIgnored, extraIgnores).Values;
        }
    }
}