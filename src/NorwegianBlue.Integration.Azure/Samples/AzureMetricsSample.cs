using NorwegianBlue.Integration.Azure.AzureAPI.DTOs.WebSiteGetHistoricalUsageMetricsResponse;
using NorwegianBlue.Samples;

namespace NorwegianBlue.Integration.Azure.Samples
{
    public class AzureMetricsSample : CommonSampleBase
    {

        public void Parse(int index, AzureHistoricalUsageMetricData azureHistoricalUsageMetricData)
        {
            // Rip out the time stamp and add to the dictionary the CPUTime and memory metrics
            TimeStamp = azureHistoricalUsageMetricData.Values[index].TimeCreated;
            var key = azureHistoricalUsageMetricData.DisplayName;
            var value = azureHistoricalUsageMetricData.Values[index].Total;

            AddParsedData(key, value);
                
        }
    }
}