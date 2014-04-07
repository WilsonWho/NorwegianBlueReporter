using System.Collections.Generic;
using System.Linq;
using NorwegianBlue.Analysis.Algorithms;
using NorwegianBlue.Integration.IIS.Samples;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Integration.IIS.Analysis
{
    public class IisSampleSetAnalysis
    {
        private readonly List<Dictionary<object, object>> _statsToGraph = new List<Dictionary<object, object>>();
        
        public IisSampleSetAnalysis()
        {
            Dictionary<object, object> configuration = YamlParser.GetConfiguration();
            var t = (List<object>)configuration["Graphs"];
            if (null != t)
            {
                _statsToGraph = t.Select(g => (Dictionary<object, object>) g).ToList();
            }
        }
               
        public void IisSummaryGraphs(IisSampleSet sampleSet)
        {
            const string title = "Azure Metrics Summary Graphs";
            const string summary = "";

            var analysisNote = CommonSampleSetAnalysis.CreateGraphs(title, summary, sampleSet, _statsToGraph);
            sampleSet.AddAnalysisNote(analysisNote);
        }
    }
}
