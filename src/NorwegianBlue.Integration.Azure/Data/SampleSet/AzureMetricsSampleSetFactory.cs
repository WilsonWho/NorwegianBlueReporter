using System.Collections.Generic;
using NorwegianBlue.Data.SampleSet;
using NorwegianBlue.Integration.Azure.Data.Sample;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Integration.Azure.Data.SampleSet
{
    public class AzureMetricsSampleSetFactory : AbstractSampleSetWithAnalysisFactory<AzureMetricsSampleSet, AzureMetricsSample>
    {
        public override AzureMetricsSampleSet Create()
        {
            var className = typeof(AzureMetricsSampleSet).Name;
            var configuration = YamlParser.GetConfiguration(className);
            return Create(configuration);
        }

        public override AzureMetricsSampleSet Create(Dictionary<object, object> configuration)
        {
            var instance = new AzureMetricsSampleSet(configuration);
            return instance;
        }
    }
}
