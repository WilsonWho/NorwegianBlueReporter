using System;
using System.Collections.Generic;
using NorwegianBlue.Data.Sample;
using NorwegianBlue.Integration.Azure.Analysis;

namespace NorwegianBlue.Integration.Azure.Data.Sample
{
    public class AzureMetricsSample : AbstractBaseSampleWithAnalysis
    {
        public override Type AnalysisNoteType
        {
            get { return typeof(AzureMetricsSampleAnalysisNote); }
        }

        internal AzureMetricsSample(DateTime timeStamp, IEnumerable<Tuple<string, string>> data, IReadOnlyDictionary<object, object> configuration ) : base(timeStamp, data, configuration)
        {
        }
    }
}