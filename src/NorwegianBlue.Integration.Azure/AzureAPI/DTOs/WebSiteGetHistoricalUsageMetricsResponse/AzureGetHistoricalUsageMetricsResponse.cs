using System.Collections.Generic;

namespace NorwegianBlue.Integration.Azure.AzureAPI.DTOs.WebSiteGetHistoricalUsageMetricsResponse
{
    public class AzureGetHistoricalUsageMetricsResponse
    {
        public IList<AzureHistoricalUsageMetric> UsageMetrics { get; set; }
    }
}
