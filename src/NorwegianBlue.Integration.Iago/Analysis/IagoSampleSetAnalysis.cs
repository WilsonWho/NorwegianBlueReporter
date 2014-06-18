using System.Collections.Generic;
using System.Linq;
using NorwegianBlue.CommonAnalyzers.Algorithms;
using NorwegianBlue.Integration.Iago.Data.SampleSet;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Integration.Iago.Analysis
{
    public class IagoSampleSetAnalysis
    {
        private readonly List<Dictionary<object, object>> _statsToGraph = new List<Dictionary<object, object>>();
        
        public IagoSampleSetAnalysis()
        {
            Dictionary<object, object> configuration = YamlParser.GetConfiguration();
            var t = (List<object>)configuration["Graphs"];
            if (null != t)
            {
                _statsToGraph = t.Select(g => (Dictionary<object, object>)g).ToList();
            }
        }
               
        public void IagoSummaryGraphs(IagoSampleSet sampleSet)
        {
            const string title = "Iago Summeries";
            const string summary = "";
            var analysisNote = CommonSampleSetAnalysis.CreateGraphs(title, summary, sampleSet, _statsToGraph);
            sampleSet.AddAnalysisNote(analysisNote);
        }
    }
}
