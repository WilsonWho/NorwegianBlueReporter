using System;
using System.Collections.Generic;

namespace NorwegianBlue.DataModels.Azure
{
    public class AzureGetHistoricalUsageMetricsResponse : NorwegianBlueDto
    {
        public IList<HistoricalUsageMetric> UsageMetrics { get; set; }
    }
}
