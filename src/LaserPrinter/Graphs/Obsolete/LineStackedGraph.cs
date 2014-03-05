using LaserOptics.Common;
using LaserPrinter.Graphs.MigraDoc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace LaserPrinter.Graphs.Obsolete
{
    public class LineStackedGraph : MigraDocGraph
    {
        public LineStackedGraph(GraphData graphData)
            : base(graphData)
        {
        }

        public override void Draw(Document document)
        {
            var chart = SetUp(ChartType.Line, document);

            chart.XAxis.MinorTickMark = TickMarkType.None;
            chart.XAxis.MajorTickMark = TickMarkType.None;
            chart.XAxis.Title.Caption = "X-Axis";

            chart.YAxis.MajorTickMark = TickMarkType.Outside;
            chart.YAxis.Title.Caption = "Y-Axis";
            chart.YAxis.HasMajorGridlines = true;

            SetGlobalChartOptions(chart);

            document.LastSection.Add(chart);
        }
    }
}