using System;
using System.Collections.Generic;

namespace NorwegianBlue.DataModels.Azure
{
    public class AzureGetHistoricalUsageMetricsRequest : NorwegianBlueDto
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public List<string> MetricNames { get; set; }
    }
}
