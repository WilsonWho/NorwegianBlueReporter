using System.Collections.Generic;
using NorwegianBlue.Analyzer;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Integration.Iago.Analysis
{
    public class IagoSampleSetAnalyzersFactory : AbstractSampleSetAnalyzersFactory<IagoSampleSetAnalyzers>
    {
        public override IagoSampleSetAnalyzers Create()
        {
            var className = typeof(IagoSampleSetAnalyzers).Name;
            var configuration = YamlParser.GetConfiguration(className);
            return Create(configuration);
        }

        public override IagoSampleSetAnalyzers Create(Dictionary<object, object> configuration)
        {
            var instance = new IagoSampleSetAnalyzers(configuration);
            return instance;
        }
    }
}
