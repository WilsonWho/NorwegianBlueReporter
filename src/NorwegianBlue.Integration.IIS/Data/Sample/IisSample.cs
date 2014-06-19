using System;
using System.Collections.Generic;
using NorwegianBlue.Data.Sample;
using NorwegianBlue.Integration.IIS.Analysis;

namespace NorwegianBlue.Integration.IIS.Data.Sample
{
    public class IisSample : AbstractBaseSampleWithAnalysis
    {
        public override Type AnalysisNoteType
        {
            get { return typeof(IisSampleAnalysisNote); }
        }

        internal IisSample(DateTime timeStamp, IEnumerable<Tuple<string, string>> data, IReadOnlyDictionary<object, object> configuration ):base(timeStamp, data, configuration)
        {
        }
    }
}