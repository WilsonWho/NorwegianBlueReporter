using System;

namespace NorwegianBlue.DataModels.Azure
{
    public class HistoricalUsageMetricSample
    {
        public DateTime TimeCreated { get; set; }
        public string Total { get; set; }
    }
}