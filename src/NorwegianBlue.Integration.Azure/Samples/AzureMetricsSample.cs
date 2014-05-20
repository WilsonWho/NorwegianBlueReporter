using System;
using System.Collections.Generic;
using NorwegianBlue.Integration.Azure.Analysis;
using NorwegianBlue.Samples;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Integration.Azure.Samples
{
    public class AzureMetricsSample : CommonSampleBase
    {
        public override Type AnalysisNoteType
        {
            get { return typeof(AzureMetricsSampleAnalysisNote); }
        }

        public AzureMetricsSample(DateTime timeStamp, IEnumerable<Tuple<string, string>> data) : base(timeStamp, data)
        {
            Dictionary<object, object> configuration = YamlParser.GetConfiguration();
            AnalysisScratchPad.FieldsToIgnore = configuration.ContainsKey("FieldsToIgnore")
                                                    ? configuration["FieldsToIgnore"]
                                                    : new List<string>(); // "FieldsToIgnore" not in config

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