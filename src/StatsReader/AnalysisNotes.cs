
namespace StatsReader
{
    public class AnalysisNote
    {
        public string Name { get; private set; }
        public string Summary { get; private set; }
        public Graph Graph { get; private set; }

        public AnalysisNote(string name, string summary, Graph graph)
        {
            Name = name;
            Summary = summary;
            Graph = graph;
        }
    }
}
