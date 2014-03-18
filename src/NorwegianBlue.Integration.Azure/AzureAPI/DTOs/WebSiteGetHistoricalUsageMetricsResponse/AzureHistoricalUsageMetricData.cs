using System.Collections.Generic;

namespace NorwegianBlue.Integration.Azure.AzureAPI.DTOs.WebSiteGetHistoricalUsageMetricsResponse
{
    public class AzureHistoricalUsageMetricData
    {
        public string DisplayName { get; set; }
        public IList<AzureHistoricalUsageMetricSample> Values { get; set; }
    }
}