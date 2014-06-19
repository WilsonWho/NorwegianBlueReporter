using System.Collections.Generic;
using NorwegianBlue.CommonAnalyzers.Algorithms;
using NorwegianBlue.Integration.IIS.Data.SampleSet;

namespace NorwegianBlue.Integration.IIS.Analysis
{
    public class IisSampleSetAnalyzers : AbstractStandardSampleSetAnalyzers<IisSampleSet>
    {
        protected override string SummmaryGraphsTitle
        {
            get { return "IIS Metrics Summary Graphs"; }
        }

        protected override string SummaryGraphsDescription
        {
            get { return ""; }
        }
        
        internal IisSampleSetAnalyzers(Dictionary<object, object> configuration) : base(configuration)
        {
        }
    }
}
