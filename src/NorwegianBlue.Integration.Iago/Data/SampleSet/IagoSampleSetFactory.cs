using System.Collections.Generic;
using NorwegianBlue.Data.SampleSet;
using NorwegianBlue.Integration.Iago.Data.Sample;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Integration.Iago.Data.SampleSet
{
    public class IagoSampleSetFactory: AbstractSampleSetWithAnalysisFactory<IagoSampleSet, IagoSample>
    {
        public override IagoSampleSet Create()
        {
            var className = typeof(IagoSampleSet).Name;
            var configuration = YamlParser.GetConfiguration(className);
            return Create(configuration);
        }

        public override IagoSampleSet Create(Dictionary<object, object> configuration)
        {
            var instance = new IagoSampleSet(configuration);
            return instance;
        }
    }
}
