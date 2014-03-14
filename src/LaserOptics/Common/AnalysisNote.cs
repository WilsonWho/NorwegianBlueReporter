using System.Collections.Generic;
using OxyPlot;

namespace LaserOptics.Common
{
    public class AnalysisNote
    {
        public string Name { get; private set; }
        public string Summary { get; private set; }
        public List<PlotModel> GraphData { get; private set; }

        public AnalysisNote(string name, string summary, PlotModel graphData)
        {
            Name = name;
            Summary = summary;
            GraphData = new List<PlotModel> {graphData};
        }

        public AnalysisNote(string name, string summary, List<PlotModel> graphData)
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
