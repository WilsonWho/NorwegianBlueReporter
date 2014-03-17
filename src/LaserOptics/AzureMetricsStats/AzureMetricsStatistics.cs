using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LaserOptics.Common;
using NorwegianBlue.Azure.DTOs.WebSiteGetHistoricalUsageMetricsResponse;

namespace LaserOptics.AzureMetricsStats
{
    public class AzureMetricsStatistics : IStatisticsAnalysis
    {
        private readonly Dictionary<string, double> _stats = new Dictionary<string, double>();
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

        public void Parse(int index, AzureHistoricalUsageMetricData azureHistoricalUsageMetricData)
        {
            // Rip out the time stamp and add to the dictionary the CPUTime and memory metrics
            TimeStamp = azureHistoricalUsageMetricData.Values[index].TimeCreated;
            var key = azureHistoricalUsageMetricData.DisplayName;
            var value = Convert.ToDouble(azureHistoricalUsageMetricData.Values[index].Total);

            _stats.Add(key, value);
                
        }

        public ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; private set; }
        public int CountFiltered(bool includeIgnored, List<string> extraIgnores)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, double>> GetEnumerator(bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key, bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out double value, bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            throw new NotImplementedException();
        }

        double IStatisticsValues.this[string key, bool includeIgnored, List<string> extraIgnores]
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> FilteredKeys(bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<double> FilteredValues(bool includeIgnored, IEnumerable<string> extraIgnores)
        {
            throw new NotImplementedException();
        }

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

        public IEnumerator<KeyValuePair<string, double>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get; private set; }
        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out double value)
        {
            throw new NotImplementedException();
        }

        double IReadOnlyDictionary<string, double>.this[string key]
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> Keys { get; private set; }
        public IEnumerable<double> Values { get; private set; }
    }
}