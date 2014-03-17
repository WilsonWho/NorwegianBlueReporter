using System.Collections.Generic;

namespace NorwegianBlue.Azure.DTOs.WebSiteGetHistoricalUsageMetricsResponse
{
    public class AzureHistoricalUsageMetricData
    {
        public string DisplayName { get; set; }
        public IList<AzureHistoricalUsageMetricSample> Values { get; set; }
    }
}