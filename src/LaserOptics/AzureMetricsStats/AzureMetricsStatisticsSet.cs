using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LaserOptics.Common;
using LaserYaml;
using NorwegianBlue.Azure;
using NorwegianBlue.DataModels;
using NorwegianBlue.DataModels.Azure;

namespace LaserOptics.AzureMetricsStats
{
    public class AzureMetricsStatisticsSet : IStatisticsSet, IStatisticsSetAnalysis
    {
        private readonly IDictionary<string, object> _configuration;

        public AzureMetricsStatisticsSet()
        {
            _configuration = YamlParser.GetConfiguration();
        }

        public void Parse()
        {
            var publishSettings = (IDictionary<object, object>) _configuration["PublishSettings"];
            var azureWebSiteManager = new AzureWebSiteManager(new PublishSettings
                {
                    ManagementCertificate = publishSettings["ManagementCert"].ToString(),
                    SubscriptionId = publishSettings["SubId"].ToString(),
                    WebSpace = publishSettings["WebSpace"].ToString(),
                    WebSite = publishSettings["WebSite"].ToString()
                });

            var azureGetHistoricalUsageMetricsDto = new AzureGetHistoricalUsageMetricsRequest
                {
                    MetricNames = new List<string> {"CpuTime", "AverageMemoryWorkingSet", "MemoryWorkingSet"},
                    StartTime = new DateTime(2014, 2, 24, 16, 13, 0),
                    EndTime = new DateTime(2014, 2, 24, 16, 40, 0),
                };

            var asdf = azureWebSiteManager.GetHistoricalUsageMetrics(azureGetHistoricalUsageMetricsDto);
        }

        public ReadOnlyCollection<IStatisticsValues> Statistics { get; private set; }
        public ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; private set; }
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
    }
}