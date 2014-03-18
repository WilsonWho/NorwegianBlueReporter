using System;

namespace NorwegianBlue.Integration.Azure.AzureAPI.DTOs.WebSiteGetHistoricalUsageMetricsResponse
{
    public class AzureHistoricalUsageMetricSample
    {
        public DateTime TimeCreated { get; set; }
        public string Total { get; set; }
    }
}