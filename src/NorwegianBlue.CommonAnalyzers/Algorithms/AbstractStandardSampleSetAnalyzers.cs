using System.Collections.Generic;
using System.Linq;
using NorwegianBlue.Analyzer;
using NorwegianBlue.Data.Sample;
using NorwegianBlue.Data.SampleSet;

namespace NorwegianBlue.CommonAnalyzers.Algorithms
{
    public abstract class AbstractStandardSampleSetAnalyzers<T> : AbstractSampleSetAnalyzers where T: ISampleSetAnalysis<ISampleAnalysis>
    {
        protected readonly List<Dictionary<object, object>> StatsToGraph = new List<Dictionary<object, object>>();

        protected abstract string SummmaryGraphsTitle { get; }
        protected abstract string SummaryGraphsDescription { get; }

        protected AbstractStandardSampleSetAnalyzers(Dictionary<object, object> configuration) : base(configuration)
        {
            var t = (List<object>) configuration["Graphs"];
            if (null != t)
            {
                StatsToGraph = t.Select(g => (Dictionary<object, object>) g).ToList();
            }
        }

        public void SummaryGraphs(T sampleSet)
        {
            var analysisNote = CommonSampleSetAnalyzers.CreateGraphs(SummmaryGraphsTitle, SummaryGraphsDescription, sampleSet, StatsToGraph);
            sampleSet.AddAnalysisNote(analysisNote);
        }
    }
}
