using System;
using LaserOptics.Common;
using LaserPrinter.Graphs.MigraDoc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace LaserPrinter.Graphs.Obsolete
{
    public class ColumnGraph : MigraDocGraph
    {
        public ColumnGraph(GraphData graphData)
            : base(graphData)
        {
            if (graphData.SeriesData.Count != 1)
            {
                throw new ArgumentException("Graph data series count is not equal to 1 ...");
            }
        }

        public override void Draw(Document document)
        {
            var chart = SetUp(ChartType.Column2D, document);

            Series series = chart.SeriesCollection.AddSeries();
            series.Add(GraphData.SeriesData[0].Data.ToArray());

            chart.XAxis.TickLabels.Format = "00";
            chart.XAxis.MajorTickMark = TickMarkType.Outside;

            chart.YAxis.MajorTickMark = TickMarkType.Outside;
            chart.YAxis.HasMajorGridlines = true;

            chart.PlotArea.LineFormat.Color = Colors.DarkGray;
            chart.PlotArea.LineFormat.Width = 1;
            chart.PlotArea.LineFormat.Visible = true;
            chart.HeaderArea.AddParagraph(GraphData.Title);
            
            SetGlobalChartOptions(chart);

            document.LastSection.Add(chart);
        }
    }
}