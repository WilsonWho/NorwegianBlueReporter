using System.Collections.Generic;
using NorwegianBlue.CommonAnalyzers.Algorithms;
using NorwegianBlue.Integration.Iago.Data.SampleSet;

namespace NorwegianBlue.Integration.Iago.Analysis
{
    public class IagoSampleSetAnalyzers : AbstractStandardSampleSetAnalyzers<IagoSampleSet>
    {
        protected override string SummmaryGraphsTitle
        {
            get { return "Iago Metrics Summary Graphs"; }
        }

        protected override string SummaryGraphsDescription
        {
            get { return ""; }
        }

        internal IagoSampleSetAnalyzers(Dictionary<object, object> configuration) : base(configuration)
        {
        }
    }
}
