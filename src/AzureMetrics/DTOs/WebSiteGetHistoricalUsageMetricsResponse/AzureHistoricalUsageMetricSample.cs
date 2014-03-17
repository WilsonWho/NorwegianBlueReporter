using System;

namespace NorwegianBlue.Azure.DTOs.WebSiteGetHistoricalUsageMetricsResponse
{
    public class AzureHistoricalUsageMetricSample
    {
        public DateTime TimeCreated { get; set; }
        public string Total { get; set; }
    }
}