using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NorwegianBlue.Analysis;
using NorwegianBlue.Analysis.Samples;
using NorwegianBlue.Integration.Azure.AzureAPI;
using NorwegianBlue.Integration.Azure.AzureAPI.DTOs.WebSiteGetHistoricalUsageMetricsResponse;
using NorwegianBlue.Util.Configuration;


namespace NorwegianBlue.Integration.Azure.Samples
{
    public class AzureMetricsSampleSet : ISampleSet, ISampleSetAnalysis
    {
        private readonly IDictionary<object, object> _configuration;

        public AzureMetricsSampleSet()
        {
            _configuration = YamlParser.GetConfiguration();
        }

        private readonly List<AzureMetricsSample> _azureMetricsStatistics = new List<AzureMetricsSample>();

        public ReadOnlyCollection<ISampleValues> Statistics { get; private set; }
        public ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; private set; }
        public DateTime ActualStartTime { get; private set; }
        public DateTime ActualEndTime { get; private set; }
        public DateTime DesiredStartTime { get; set; }
        public DateTime DesiredEndTime { get; set; }

        public ISampleValues GetNearest(DateTime time)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ReadOnlyDictionary<string, double>> ExportStatistics(bool firstRowHeaders = true, string defValue = "missing")
        {
            throw new System.NotImplementedException();
        }

        public dynamic AnalysisScratchPad { get; private set; }
        public void AddAnalysisNote(AnalysisNote note)
        {
            throw new System.NotImplementedException();
        }

        public void Analyze(IEnumerable<SetAnalyzer> setAnalyzers, IEnumerable<StatAnalyzer> statAnalyzers)
        {
            throw new System.NotImplementedException();
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
                    var azureMetricsStatistic = new AzureMetricsSample();

                    foreach (var azureHistoricalUsageMetric in azureResponse.UsageMetrics)
                    {
                        azureMetricsStatistic.Parse(i, azureHistoricalUsageMetric.Data);
                    }

                    _azureMetricsStatistics.Add(azureMetricsStatistic);
                }
            }
        }

        public IEnumerator<ISampleValues> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(ISampleValues item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(ISampleValues item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(ISampleValues[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(ISampleValues item)
        {
            throw new NotImplementedException();
        }

        public int Count { get; private set; }
        public bool IsReadOnly { get; private set; }
        public int IndexOf(ISampleValues item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, ISampleValues item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public ISampleValues this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}