
using System.Collections.Generic;

namespace LaserOptics
{
    public class AnalysisNote
    {
        public string Name { get; private set; }
        public string Summary { get; private set; }
        public List<GraphData> GraphData { get; private set; }

        public AnalysisNote(string name, string summary, GraphData graphData)
        {
            Name = name;
            Summary = summary;
            GraphData = new List<GraphData> {graphData};
        }

        public AnalysisNote(string name, string summary, List<GraphData> graphData)
        {
            Name = name;
            Summary = summary;
            GraphData = graphData;
        }

        public AnalysisNote(string name, string summary)
        {
            Name = name;
            Summary = summary;
            GraphData = null;
        }
    }
}
