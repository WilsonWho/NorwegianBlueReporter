using System.Collections.Generic;

namespace NorwegianBlue.Azure.DTOs.WebSiteGetHistoricalUsageMetricsResponse
{
    public class AzureGetHistoricalUsageMetricsResponse
    {
        public IList<AzureHistoricalUsageMetric> UsageMetrics { get; set; }
    }
}
