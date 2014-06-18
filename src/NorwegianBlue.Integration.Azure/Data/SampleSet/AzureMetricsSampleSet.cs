using System;
using System.Collections.Generic;
using System.Linq;
using NorwegianBlue.Data.SampleSet;
using NorwegianBlue.Integration.Azure.Analysis;
using NorwegianBlue.Integration.Azure.AzureAPI;
using NorwegianBlue.Integration.Azure.AzureAPI.DTOs.WebSiteGetHistoricalUsageMetricsResponse;
using NorwegianBlue.Integration.Azure.Data.Sample;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Integration.Azure.Data.SampleSet
{
    public class AzureMetricsSampleSet : BaseSampleSetWithAnalysis<AzureMetricsSample>
    {
        public override Type AnalysisNoteType
        {
            get { return typeof(AzureMetricsSampleSetAnalysisNote); }
        }

        internal AzureMetricsSampleSet(IReadOnlyDictionary<object, object> configuration) : base(configuration)
        {
        }

        protected override IEnumerable<AzureMetricsSample> DoParse(TimeZone desiredTimeZone, DateTime? startTime, DateTime? endTime, dynamic dataSourceConfigObject)
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

            var azureMetricsSampleFactory = new AzureMetricsSampleFactory();

            var azureMetricsSamples = data.Select(kvp => azureMetricsSampleFactory.Create(kvp.Key, kvp.Value));
            return azureMetricsSamples;
        }
    }
}