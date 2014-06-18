using System;
using System.Collections.Generic;
using NorwegianBlue.Data.Sample;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Integration.Azure.Data.Sample
{
    class AzureMetricsSampleFactory : AbstractSampleWithAnalysisFactory<AzureMetricsSample>
    {
        public override AzureMetricsSample Create(DateTime timeStamp, IEnumerable<Tuple<string, string>> data)
        {
            var className = typeof (AzureMetricsSample).Name;
            var configuration = YamlParser.GetConfiguration(className);
            return Create(timeStamp, data, configuration);
        }

        public override AzureMetricsSample Create(DateTime timeStamp, IEnumerable<Tuple<string, string>> data, Dictionary<object, object> configuration)
        {
            var instance = new AzureMetricsSample(timeStamp, data, configuration);
            return instance;
        }
    }
}
