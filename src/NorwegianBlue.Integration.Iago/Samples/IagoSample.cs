using System;
using System.Collections.Generic;
using NorwegianBlue.IagoIntegration.Analysis;
using NorwegianBlue.Samples;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.IagoIntegration.Samples
{
    public class IagoSample : CommonSampleBase 
    {
        public override Type AnalysisNoteType
        {
            get { return typeof(IagoSampleAnalysisNote); }
        }

        public IagoSample(DateTime timeStamp, IEnumerable<Tuple<string, string>> data):base(timeStamp, data)
        {
            Dictionary<object, object> configuration = YamlParser.GetConfiguration();
            AnalysisScratchPad.FieldsToIgnore = configuration.ContainsKey("FieldsToIgnore")
                                                    ? configuration["FieldsToIgnore"]
                                                    : new List<string>(); // "FiledsToIgnore" might be absent in the config file

            // FieldsToIgnore might be defined but empty in the config...
            if (null == AnalysisScratchPad.FieldsToIgnore)
            {
                AnalysisScratchPad.FieldsToIgnore = new List<string>();
            }

            TimeStamp = timeStamp;
            foreach (var tuple in data)
            {
                AddParsedData(tuple.Item1, tuple.Item2);
            }
        }
    }
}
