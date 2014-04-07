using System.Collections.Generic;
using System.Linq;
using NorwegianBlue.Analysis.Algorithms;
using NorwegianBlue.Integration.Azure.Samples;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Integration.Azure.Analysis
{
    public class AzureMetricsSampleSetAnalysis
    {
        private readonly List<Dictionary<object, object>> _statsToGraph = new List<Dictionary<object, object>>();
        
        public AzureMetricsSampleSetAnalysis()
        {
            Dictionary<object, object> configuration = YamlParser.GetConfiguration();
            var t = (List<object>)configuration["Graphs"];
            if (null != t)
            {
                _statsToGraph = t.Select(g => (Dictionary<object, object>) g).ToList();
            }
        }
               
        public void AzureMetricsSummaryGraphs(AzureMetricsSampleSet sampleSet)
        {
            const string title = "Azure Metrics Summary Graphs";
            const string summary = "";

            var analysisNote = CommonSampleSetAnalysis.CreateGraphs(title, summary, sampleSet, _statsToGraph);
            sampleSet.AddAnalysisNote(analysisNote);
        }
    }
}
