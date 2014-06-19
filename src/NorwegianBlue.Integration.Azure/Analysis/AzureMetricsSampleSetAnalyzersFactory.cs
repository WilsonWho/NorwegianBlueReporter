using System.Collections.Generic;
using NorwegianBlue.Analyzer;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Integration.Azure.Analysis
{
    public class AzureMetricsSampleSetAnalyzersFactory : AbstractSampleSetAnalyzersFactory<AzureMetricsSampleSetAnalyzers>
    {
        public override AzureMetricsSampleSetAnalyzers Create()
        {
            var className = typeof (AzureMetricsSampleSetAnalyzers).Name;
            var configuration = YamlParser.GetConfiguration(className);
            return Create(configuration);
        }

        public override AzureMetricsSampleSetAnalyzers Create(Dictionary<object, object> configuration)
        {
            var instance = new AzureMetricsSampleSetAnalyzers(configuration);
            return instance;
        }
    }
}
