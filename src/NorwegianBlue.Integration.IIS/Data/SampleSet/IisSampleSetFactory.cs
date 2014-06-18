using System.Collections.Generic;
using NorwegianBlue.Data.SampleSet;
using NorwegianBlue.Integration.IIS.Data.Sample;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Integration.IIS.Data.SampleSet
{
    public class IisSampleSetFactory : AbstractSampleSetWithAnalysisFactory<IisSampleSet, IisSample>
    {
        public override IisSampleSet Create()
        {
            var className = typeof(IisSampleSet).Name;
            var configuration = YamlParser.GetConfiguration(className);
            return Create(configuration);
        }

        public override IisSampleSet Create(Dictionary<object, object> configuration)
        {
            var instance = new IisSampleSet(configuration);
            return instance;
        }
    }
}
