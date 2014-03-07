using LaserOptics;
using LaserOptics.Common;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace LaserPrinter.Graphs.MigraDoc
{
    public class MigraDocColumnGraph : MigraDocGraph
    {
        public MigraDocColumnGraph(GraphData graphData) : base(graphData)
        {
        }

        public override void Draw(Document document)
        {
            var chart = SetUp(ChartType.Column2D, document);

            SetGlobalChartOptions(chart);
            document.LastSection.Add(chart);
        }
    }
}