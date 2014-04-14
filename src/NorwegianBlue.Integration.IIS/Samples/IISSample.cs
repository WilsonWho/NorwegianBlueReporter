using System;
using System.Collections.Generic;
using NorwegianBlue.Samples;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Integration.IIS.Samples
{
    public class IisSample : CommonSampleBase
    {
        public IisSample(DateTime timeStamp, IEnumerable<Tuple<string, string>> data):base(timeStamp, data)
        {
            Dictionary<object, object> configuration = YamlParser.GetConfiguration();
            AnalysisScratchPad.FieldsToIgnore = configuration.ContainsKey("FieldsToIgnore")
                                                    ? configuration["FieldsToIgnore"]
                                                    : new List<string>();

            TimeStamp = timeStamp;
            foreach (var tuple in data)
            {
                AddParsedData(tuple.Item1, tuple.Item2);
            }
        }
    }
}