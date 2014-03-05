using LaserOptics.Common;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace LaserPrinter.Graphs
{
    public class ColumnStackedGraph : Graph
    {
        public ColumnStackedGraph(GraphData graphData)
            : base(graphData)
        {
        }

        public override void Draw(Document document)
        {
            var chart = SetUp(ChartType.ColumnStacked2D, document);

            foreach (var seriesData in GraphData.SeriesData)
            {
                Series series = chart.SeriesCollection.AddSeries();
                series.Name = seriesData.Name;
                series.Add(seriesData.Data.ToArray());
            }

            chart.XAxis.TickLabels.Format = "00";
            chart.XAxis.MajorTickMark = TickMarkType.Outside;
            chart.XAxis.Title.Caption = "X-Axis";

            chart.YAxis.MajorTickMark = TickMarkType.Outside;
            chart.YAxis.HasMajorGridlines = true;

            chart.PlotArea.LineFormat.Color = Colors.DarkGray;
            chart.PlotArea.LineFormat.Width = 1;
            chart.PlotArea.LineFormat.Visible = true;

            SetGlobalChartOptions(chart);

            document.LastSection.Add(chart);
        }
    }
}