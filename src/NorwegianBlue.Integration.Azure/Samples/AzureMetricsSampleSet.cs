using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using NorwegianBlue.Analysis;
using NorwegianBlue.Analysis.Samples;
using NorwegianBlue.Integration.Azure.AzureAPI;
using NorwegianBlue.Integration.Azure.AzureAPI.DTOs.WebSiteGetHistoricalUsageMetricsResponse;
using NorwegianBlue.Samples;
using NorwegianBlue.Util.Configuration;


namespace NorwegianBlue.Integration.Azure.Samples
{
    public class AzureMetricsSampleSet : ISampleSet<AzureMetricsSample>, ISampleSetAnalysis<AzureMetricsSample>
    {
        private readonly IDictionary<object, object> _configuration;

        private readonly List<AzureMetricsSample> _azureMetricsSamples = new List<AzureMetricsSample>();

        private readonly dynamic _analysisScratchPad = new ExpandoObject();
        public dynamic AnalysisScratchPad
        {
            get { return _analysisScratchPad; }
        }

        private readonly List<AnalysisNote> _analysisNotes = new List<AnalysisNote>();
        private ReadOnlyCollection<AnalysisNote> _roAnalysisNote;

        public ReadOnlyCollection<AnalysisNote> AnalysisNotes
        {
            get { return _roAnalysisNote ?? (_roAnalysisNote = new ReadOnlyCollection<AnalysisNote>(_analysisNotes)); }
        }

        public DateTime StartTime
        {
            get
            {
                if (0 == _azureMetricsSamples.Count)
                {
                    throw new DataException("No samples in collection");
                }
                return _azureMetricsSamples.First().TimeStamp;
            }
        }

        public DateTime EndTime
        {
            get
            {
                if (0 == _azureMetricsSamples.Count)
                {
                    throw new DataException("No samples in collection");
                }
                return _azureMetricsSamples.Last().TimeStamp;
            }
        }

        public int Count {
            get { return _azureMetricsSamples.Count; }
        }

        AzureMetricsSample IReadOnlyList<AzureMetricsSample>.this[int index]
        {
            get { return _azureMetricsSamples[index]; }
        }
        
        AzureMetricsSample ISampleSetValues<AzureMetricsSample>.this[DateTime time]
        {
            get { return SampleSetComparisons<AzureMetricsSample>.GetNearestToTime(_azureMetricsSamples, time); }
        }

        AzureMetricsSample ISampleSetValues<AzureMetricsSample>.this[DateTime time, TimeSpan tolerance, bool absolute]
        {
            get { return SampleSetComparisons<AzureMetricsSample>.GetNearestToTime(_azureMetricsSamples, time, tolerance, absolute); }
        }

        IEnumerator<AzureMetricsSample> IEnumerable<AzureMetricsSample>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<ISampleValues> GetEnumerator()
        {
            return _azureMetricsSamples.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public AzureMetricsSampleSet()
        {
            _configuration = YamlParser.GetConfiguration();
        }

        public void Parse(TimeZone timeZone, string dataLocation, DateTime? startTime, DateTime? endTime)
        {
            var publishSettings = (IDictionary<object, object>)_configuration["PublishSettings"];
            var azureWebSiteManager = new AzureWebSiteManager(new PublishSettings
            {
                ManagementCertificate = publishSettings["ManagementCert"].ToString(),
                SubscriptionId = publishSettings["SubId"].ToString(),
                WebSpace = publishSettings["WebSpace"].ToString(),
                WebSite = publishSettings["WebSite"].ToString()
            });

            var azureGetHistoricalUsageMetricsDto = new AzureGetHistoricalUsageMetricsRequest
            {
                MetricNames = new List<string> { "CpuTime", "AverageMemoryWorkingSet", "MemoryWorkingSet" },
                StartTime = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(startTime)),
                EndTime = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(endTime)),
            };

            var azureResponse = azureWebSiteManager.GetHistoricalUsageMetrics(azureGetHistoricalUsageMetricsDto);

            if (azureResponse.UsageMetrics.Count > 0)
            {
                var length = azureResponse.UsageMetrics[0].Data.Values.Count;

                for (int i = 0; i < length; i++)
                {
                    var azureMetricsSample = new AzureMetricsSample();

                    foreach (var azureHistoricalUsageMetric in azureResponse.UsageMetrics)
                    {
                        azureMetricsSample.Parse(i, azureHistoricalUsageMetric.Data);
                    }

                    _azureMetricsSamples.Add(azureMetricsSample);
                }
            }
        }

        public void AddAnalysisNote(AnalysisNote note)
        {
            _analysisNotes.Add(note);
        }

//        public void Analyze(IEnumerable<SetAnalyzer<ISampleSetAnalysis<AzureMetricsSample>, AzureMetricsSample>> setAnalyzers,
//                            IEnumerable<SampleInSetAnalyzer<ISampleSetAnalysis<AzureMetricsSample>, AzureMetricsSample>> statAnalyzers)
        public void Analyze(dynamic setAnalyzers, dynamic statAnalyzers)
        {
            foreach (var analyzer in setAnalyzers)
            {
                analyzer.Invoke(this);
            }

            foreach (var stat in _azureMetricsSamples)
            {
                foreach (var analyzer in statAnalyzers)
                {
                    analyzer.Invoke(this, stat);
                }
            }
        }

        public ReadOnlyCollection<ReadOnlyDictionary<string, double>> ExportStatistics(bool firstRowHeaders = true, string defValue = "missing")
        {
            throw new System.NotImplementedException();
        }

    }
}