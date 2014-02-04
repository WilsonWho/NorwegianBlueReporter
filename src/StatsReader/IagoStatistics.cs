using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace StatsReader
{
    class IagoStatistics : IStatistics, IStatisticsValues, IStatisticsAddAnalysis, IStatisticsSelfAnalysis
    {
        // Example line:
        // INF [20140129-16:09:01.218] stats: {...}
        private static readonly Regex LineMatcher = new Regex(@"^INF \[(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})-(?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2})\.(?<millis>\d{3})\] stats: \{(?<stats>.*)\}$", RegexOptions.Compiled);
        private static readonly Regex DataMatcher = new Regex(@"(?<key>.+?):(?<value>.+?)[,$]", RegexOptions.Compiled);
        private readonly dynamic _analysisScratchPad = new ExpandoObject();

        private readonly float _iagoStatAllowedRequestResponseDifference = float.Parse(ConfigurationManager.AppSettings["IagoStatAllowedRequestResponseDifference"], CultureInfo.InvariantCulture.NumberFormat);

        private readonly Dictionary<String, String> _stats = new Dictionary<string, string>();
        private readonly List<AnalysisNote> _analysisNotes = new List<AnalysisNote>(); 
        public DateTime TimeStamp { get ; private set; }

        ReadOnlyDictionary<String, String> IStatisticsValues.Stats
        {
            get { return Utils.ReadOnlyDictionaryWithCopiedValues(_stats);}  
        }

        ReadOnlyCollection<AnalysisNote> IStatisticsValues.AnalysisNotes
        {
            get { return Utils.ReadOnlyCollectionWithCopiedValues(_analysisNotes); }
        }

        dynamic IStatisticsAddAnalysis.AnalysisScratchPad { get { return _analysisScratchPad; } }

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
                String value = kvpmatch.Groups["value"].Value;

                if (!_stats.ContainsKey(key))
                {
                    _stats[key] = value;
                }
                else
                {
                    throw new InvalidDataException(
                        "input line has duplicate data intems and can't be parsed by Iago parser. Data name: " + key +
                        ", line: " + input);
                }
            }
        }

        public void Analyze()
        {
            // TODO
        }

        public void AddAnalysisNote(AnalysisNote note)
        {
            _analysisNotes.Add(note);
        }

    }
}
