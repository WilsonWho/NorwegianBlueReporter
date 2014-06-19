using System.Collections.Generic;
using NorwegianBlue.CommonAnalyzers.Algorithms;
using NorwegianBlue.Integration.Azure.Data.SampleSet;

namespace NorwegianBlue.Integration.Azure.Analysis
{
    public class AzureMetricsSampleSetAnalyzers : AbstractStandardSampleSetAnalyzers<AzureMetricsSampleSet>
    {
        protected override string SummmaryGraphsTitle
        {
            get { return "Azure Metrics Summary Graphs"; }
        }

        protected override string SummaryGraphsDescription
        {
            get { return ""; }
        }
                
        internal AzureMetricsSampleSetAnalyzers(Dictionary<object, object> configuration ) : base(configuration)
        {
        }
    }
}
