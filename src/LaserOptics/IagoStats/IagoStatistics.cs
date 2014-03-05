using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Text.RegularExpressions;
using LaserOptics.Common;
using LaserYaml;
using LaserYaml.DTOs;

namespace LaserOptics.IagoStats
{
    class IagoStatistics : IStatistics, IStatisticsAnalysis
    {
        private Rule _rule;

        // Example line:
        // INF [20140129-16:09:01.218] stats: {...}
        private static readonly Regex LineMatcher = new Regex(@"^INF \[(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})-(?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2})\.(?<millis>\d{3})\] stats: \{(?<stats>.*)\}$", RegexOptions.Compiled);
        private static readonly Regex DataMatcher = new Regex(@"(?<key>.+?):(?<value>.+?)[,$]", RegexOptions.Compiled);
        private static readonly Regex QuotteRemover = new Regex("([\"'])(?<key>.+?)\\1", RegexOptions.Compiled);
        private static readonly Regex VSlashRemover = new Regex("\\\\/", RegexOptions.Compiled);

        private readonly dynamic _analysisScratchPad = new ExpandoObject();

        private readonly Dictionary<String, double> _stats = new Dictionary<string, double>();
        private ReadOnlyDictionary<String, double> _roStats;

        private readonly Dictionary<String, string> _nonStats = new Dictionary<string, string>();
        private ReadOnlyDictionary<String, string> _roNonStats;

        private readonly List<AnalysisNote> _analysisNotes = new List<AnalysisNote>();
        private ReadOnlyCollection<AnalysisNote> _roAnalysisNotes;
        public DateTime TimeStamp { get ; private set; }

        public IagoStatistics(Configuration configuration)
        {
            _rule = configuration.GetConfigurationFor<IagoStatistics>();
        }

        public ReadOnlyDictionary<string, double> Stats
        {
            get { return _roStats ?? (_roStats = new ReadOnlyDictionary<String, double>(_stats)); }
        }

        public ReadOnlyDictionary<string, string> NonStats
        {
            get { return _roNonStats ?? (_roNonStats = new ReadOnlyDictionary<String, String>(_nonStats)); }
        }

        public ReadOnlyCollection<AnalysisNote> AnalysisNotes
        {
            get { return _roAnalysisNotes ?? (_roAnalysisNotes = new ReadOnlyCollection<AnalysisNote>(_analysisNotes)); }
        }

        public dynamic AnalysisScratchPad { get { return _analysisScratchPad; } }

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

                if (!_stats.ContainsKey(key))
                {
                    double d;
                    if(double.TryParse(value, out d))
                    {
                        _stats[key] = d;
                    }
                    else
                    {
                        _nonStats[key] = value;
                    }
                }
                else
                {
                    throw new InvalidDataException(
                        "input line has duplicate data intems and can't be parsed by Iago parser. Data name: " + key +
                        ", line: " + input);
                }
            }
        }

        public void Analyze(IEnumerable<StatAnalyzer> analyzers)
        {
            // TODO
        }

        public void AddAnalysisNote(AnalysisNote note)
        {
            _analysisNotes.Add(note);
        }
    }
}
