using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LaserOptics.Common;

namespace LaserOptics.AzureMetricsStats
{
    public class AzureMetricsStatistics : IStatisticsAnalysis
    {
        public void Parse(string input)
        {
            throw new NotImplementedException();
        }

        private readonly Dictionary<String, double> _stats = new Dictionary<string, double>();
        private ReadOnlyDictionary<String, double> _roStats;

        private readonly Dictionary<String, string> _nonStats = new Dictionary<string, string>();
        private ReadOnlyDictionary<String, string> _roNonStats;

        public DateTime TimeStamp { get; private set; }

        public ReadOnlyDictionary<string, double> Stats
        {
            get { return _roStats ?? (_roStats = new ReadOnlyDictionary<String, double>(_stats)); }
        }

        public ReadOnlyDictionary<string, string> NonStats
        {
            get { return _roNonStats ?? (_roNonStats = new ReadOnlyDictionary<String, String>(_nonStats)); }
        }

        public ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; private set; }
        public dynamic AnalysisScratchPad { get; private set; }

        private readonly List<AnalysisNote> _analysisNotes = new List<AnalysisNote>();

        public void AddAnalysisNote(AnalysisNote note)
        {
            _analysisNotes.Add(note);
        }

        public void Analyze(IEnumerable<StatAnalyzer> analyzers)
        {
            throw new NotImplementedException();
        }
    }
}