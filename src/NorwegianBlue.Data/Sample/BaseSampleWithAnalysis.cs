using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NorwegianBlue.Notes.AnalysisNotes;

namespace NorwegianBlue.Data.Sample
{
    public abstract class BaseSampleWithAnalysis : ISampleAnalysis
    {
        protected readonly Dictionary<string, Tuple<bool, double>> Stats = new Dictionary<string, Tuple<bool, double>>();
        protected readonly List<AnalysisNote> UpdateableAnalysisNotes = new List<AnalysisNote>();

        protected readonly Dictionary<string, Tuple<bool, string>> UpdateableNonStats =
            new Dictionary<string, Tuple<bool, string>>();

        private readonly dynamic _analysisScratchPad = new ExpandoObject();
        private ReadOnlyCollection<AnalysisNote> _roAnalysisNotes;

        private ReadOnlyDictionary<string, Tuple<bool, string>> _roNonStats;

        public DateTime TimeStamp { get; protected set; }

        public abstract Type AnalysisNoteType { get; }

        public dynamic AnalysisScratchPad
        {
            get { return _analysisScratchPad; }
        }

        public virtual ReadOnlyDictionary<string, Tuple<bool, string>> NonStats
        {
            get
            {
                return _roNonStats ??
                       (_roNonStats = new ReadOnlyDictionary<string, Tuple<bool, string>>(UpdateableNonStats));
            }
        }

        public virtual ReadOnlyCollection<AnalysisNote> AnalysisNotes
        {
            get
            {
                return _roAnalysisNotes ??
                       (_roAnalysisNotes = new ReadOnlyCollection<AnalysisNote>(UpdateableAnalysisNotes));
            }
        }

        protected BaseSampleWithAnalysis(DateTime timeStamp, IEnumerable<Tuple<string, string>> data, IReadOnlyDictionary<object, object> configuration)
        {
            TimeStamp = timeStamp;

            AnalysisScratchPad.FieldsToIgnore = (configuration.ContainsKey("FieldsToIgnore")
                                                     ? configuration["FieldsToIgnore"]
                                                     : new List<string>())    // "FiledsToIgnore" might be absent in the config file
                                                ?? new List<string>();   // FieldsToIgnore might be defined but empty in the config...

            foreach (var tuple in data)
            {
                AddParsedData(tuple.Item1, tuple.Item2);
            }
        }


        public virtual void Analyze(IEnumerable<SampleAnalyzer<ISampleAnalysis>> analyzers)
        {
            foreach (var statAnalyzer in analyzers)
            {
                statAnalyzer.Invoke(this);
            }
        }

        public virtual void AddAnalysisNote(AnalysisNote note)
        {
            UpdateableAnalysisNotes.Add(note);
        }

        public IEnumerator<KeyValuePair<string, double>> GetEnumerator(bool includeIgnored,
                                                                       IEnumerable<string> extraIgnores)
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

        public int Count
        {
            get { return CountFiltered(false, null); }
        }

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

        public double this[string key]
        {
            get { return this[key, false, null]; }
        }

        public double this[string key, bool includeIgnored, IEnumerable<string> extraIgnores]
        {
            get { return FilterStats(includeIgnored, extraIgnores)[key]; }
        }

        public IEnumerable<string> Keys
        {
            get { return FilteredKeys(false, null); }
        }

        public IEnumerable<string> FilteredKeys(bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            return FilterStats(includeIgnored, extraIgnores).Keys;
        }

        public IEnumerable<double> Values
        {
            get { return FilteredValues(false, null); }
        }

        public IEnumerable<double> FilteredValues(bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            return FilterStats(includeIgnored, extraIgnores).Values;
        }

        protected void AddParsedData(string key, string value)
        {
            if (Stats.ContainsKey(key) || UpdateableNonStats.ContainsKey(key))
            {
                throw new InvalidDataException(
                    "input line has duplicate data intems and can't be parsed by Iago parser. Data name: " + key +
                    ", value: " + value);
            }

            bool ignore = false;
            foreach (dynamic ignoreRegex in AnalysisScratchPad.FieldsToIgnore)
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
                Stats[key] = new Tuple<bool, double>(ignore, d);
            }
            else
            {
                UpdateableNonStats[key] = new Tuple<bool, string>(ignore, value);
            }
        }

        protected Dictionary<string, double> FilterStats(bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            //If there's a performance issue, cache the filtered dictionary, including the extra ignore list and see if this can be used in the next call.
            Dictionary<string, double> filteredStats;

            if (includeIgnored)
            {
                filteredStats = Stats.ToDictionary(stat => stat.Key, stat => stat.Value.Item2);
            }
            else
            {
                filteredStats = Stats.Where(stat => !stat.Value.Item1)
                                     .ToDictionary(stat => stat.Key, stat => stat.Value.Item2);
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
    }
}