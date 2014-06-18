using System;
using System.Collections.Generic;
using NorwegianBlue.Data.Sample;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Integration.IIS.Data.Sample
{
    class IisSampleFactory : AbstractSampleWithAnalysisFactory<IisSample>
    {
        public override IisSample Create(DateTime timeStamp, IEnumerable<Tuple<string, string>> data)
        {
            var className = typeof(IisSample).Name;
            var configuration = YamlParser.GetConfiguration(className);
            return Create(timeStamp, data, configuration);

        }

        public override IisSample Create(DateTime timeStamp, IEnumerable<Tuple<string, string>> data, Dictionary<object, object> configuration)
        {
            var instance = new IisSample(timeStamp, data, configuration);
            return instance;
        }
    }
}
