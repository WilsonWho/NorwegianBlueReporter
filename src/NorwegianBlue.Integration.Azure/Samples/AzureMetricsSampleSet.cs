using System;
using System.Collections.Generic;
using System.Linq;
using NorwegianBlue.Integration.Azure.AzureAPI;
using NorwegianBlue.Integration.Azure.AzureAPI.DTOs.WebSiteGetHistoricalUsageMetricsResponse;
using NorwegianBlue.Samples;
using NorwegianBlue.Util.Configuration;


namespace NorwegianBlue.Integration.Azure.Samples
{
    public class AzureMetricsSampleSet : CommonSampleSetBase<AzureMetricsSample>
    {

        public AzureMetricsSampleSet()
        {
            Dictionary<object, object> configuration = YamlParser.GetConfiguration();

            AnalysisScratchPad.ignorableFields = configuration.ContainsKey("FieldsToIgnore")
                                                     ? configuration["FieldsToIgnore"]
                                                     : new List<string>();
        }

        public override List<AzureMetricsSample> DoParse(TimeZone desiredTimeZone, DateTime? startTime, DateTime? endTime, dynamic dataSourceConfigObject)
        {
            IDictionary<object, object> configuration = YamlParser.GetConfiguration();
            var fieldNames = ((List<object>)configuration["Fields"]).Select( s => s.ToString()).ToList();
            var publishSettingsConfig = (IDictionary<object, object>)configuration["PublishSettings"];
            var azureWebSiteManager = new AzureWebSiteManager(new PublishSettings
            {
                ManagementCertificate = publishSettingsConfig["ManagementCert"].ToString(),
                SubscriptionId = publishSettingsConfig["SubId"].ToString(),
                WebSpace = publishSettingsConfig["WebSpace"].ToString(),
                WebSite = publishSettingsConfig["WebSite"].ToString()
            });

            var azureGetHistoricalUsageMetricsRequestDto = new AzureGetHistoricalUsageMetricsRequest
            {
                MetricNames = fieldNames,
                StartTime = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(startTime)),
                EndTime = TimeZoneInfo.ConvertTimeToUtc(Convert.ToDateTime(endTime)),
            };

            var azureResponse = azureWebSiteManager.GetHistoricalUsageMetrics(azureGetHistoricalUsageMetricsRequestDto);

            // the returned data structure is convoluted: collection of metrics (eg "CPUTime")
            // each containing an data object with a display name, and under that, a collection of values for each timestamp.
            // Have to transform this to a collection of timestamps, each containing a collection of values.

            if (0 == azureResponse.UsageMetrics.Count)
            {
                throw new ApplicationException("No Azure metrics returned.");
            }

            var data = new Dictionary<DateTime, List<Tuple<string, string>>>();

            foreach (var azureHistoricalUsageMetric in azureResponse.UsageMetrics)
            {
                var metricDisplayName = azureHistoricalUsageMetric.Data.DisplayName;
                foreach (var timeValue in azureHistoricalUsageMetric.Data.Values)
                {
                    var timeStamp = timeValue.TimeCreated;
                    var value = timeValue.Total;

                    if (!data.ContainsKey(timeStamp))
                    {
                        data[timeStamp] = new List<Tuple<string, string>>();
                    }

                    var dataTuple = new Tuple<string, string>(metricDisplayName, value);
                    data[timeStamp].Add(dataTuple);
                }
            }

            var azureMetricsSamples = data.Select(kvp => new AzureMetricsSample(kvp.Key, kvp.Value)).ToList();

            return azureMetricsSamples;
        }
    }
}