using System;
using System.Collections.Generic;
using NorwegianBlue.Data.Sample;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Integration.Iago.Data.Sample
{
    class IagoSampleFactory : AbstractSampleWithAnalysisFactory<IagoSample>
    {
        public override IagoSample Create(DateTime timeStamp, IEnumerable<Tuple<string, string>> data)
        {
            var className = typeof (IagoSample).Name;
            var configuration = YamlParser.GetConfiguration(className);
            return Create(timeStamp, data, configuration);
        }

        public override IagoSample Create(DateTime timeStamp, IEnumerable<Tuple<string, string>> data, Dictionary<object, object> configuration)
        {
            var instance = new IagoSample(timeStamp, data, configuration);
            return instance;
        }
    }
}
