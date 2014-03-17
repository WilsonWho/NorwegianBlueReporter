using System.Collections.Generic;

namespace NorwegianBlue.DataModels.Azure
{
    public class HistoricalUsageMetricData
    {
        public string DisplayName { get; set; }
        public IList<HistoricalUsageMetricSample> Values { get; set; }
    }
}