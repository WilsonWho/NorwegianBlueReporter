
namespace StatsReader
{
    public class AnalysisNote
    {
        public string Name { get; private set; }
        public string Summary { get; private set; }
        public GraphData GraphData { get; private set; }

        public AnalysisNote(string name, string summary, GraphData graphData)
        {
            Name = name;
            Summary = summary;
            GraphData = graphData;
        }
    }
}
