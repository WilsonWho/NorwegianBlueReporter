using LaserOptics;
using LaserOptics.Common;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace LaserPrinter.Graphs.MigraDoc
{
    public class MigraDocLineGraph : MigraDocGraph
    {
        public MigraDocLineGraph(GraphData graphData) : base(graphData)
        {
        }

        public override void Draw(Document document)
        {
            var chart = SetUp(ChartType.Line, document);

            SetGlobalChartOptions(chart);
            document.LastSection.Add(chart);
        }
    }
}