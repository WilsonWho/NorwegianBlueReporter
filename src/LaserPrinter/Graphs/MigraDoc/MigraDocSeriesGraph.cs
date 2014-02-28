using LaserOptics;
using MigraDoc.DocumentObjectModel;

namespace LaserPrinter.Graphs.MigraDoc
{
    public class MigraDocSeriesGraph : MigraDocGraph
    {
        public MigraDocSeriesGraph(GraphData graphData) : base(graphData)
        {
        }

        public override void Draw(Document document)
        {
            var chart = SetUp(Convert(GraphData.GraphType), document);
            SetGlobalChartOptions(chart);

            document.LastSection.Add(chart);
        }
    }
}