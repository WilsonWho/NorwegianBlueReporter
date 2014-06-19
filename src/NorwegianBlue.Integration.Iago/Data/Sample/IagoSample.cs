using System;
using System.Collections.Generic;
using NorwegianBlue.Data.Sample;
using NorwegianBlue.Integration.Iago.Analysis;

namespace NorwegianBlue.Integration.Iago.Data.Sample
{
    public class IagoSample : AbstractBaseSampleWithAnalysis 
    {
        public override Type AnalysisNoteType
        {
            get { return typeof(IagoSampleAnalysisNote); }
        }

        internal IagoSample(DateTime timeStamp, IEnumerable<Tuple<string, string>> data, IReadOnlyDictionary<object, object> configuration ):base(timeStamp, data, configuration)
        {
        }
    }
}
