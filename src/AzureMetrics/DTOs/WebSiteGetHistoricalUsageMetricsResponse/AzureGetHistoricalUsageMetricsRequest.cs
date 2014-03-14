using System;
using System.Collections.Generic;

namespace NorwegianBlue.Azure.DTOs.WebSiteGetHistoricalUsageMetricsResponse
{
    public class AzureGetHistoricalUsageMetricsRequest
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public List<string> MetricNames { get; set; }
    }
}
