using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace ReportWriter
{
    public class GraphManager : IGraphManager
    {
        // TODO: make this customizable
         public void DefineChart(Document document)
         {
             document.AddSection();

             Paragraph paragraph = document.LastSection.AddParagraph("Chart Overview", "Heading1");
             paragraph.AddBookmark("Charts");

             document.LastSection.AddParagraph("Sample Chart", "Heading2");

             var chart = new Chart
                 {
                     Left = 0,
                     Width = Unit.FromCentimeter(16),
                     Height = Unit.FromCentimeter(12)
                 };

             var series = chart.SeriesCollection.AddSeries();
             series.ChartType = ChartType.Column2D;
             series.Add(new double[] {1, 17, 45, 5, 3, 20, 11, 23, 8, 19});
             series.HasDataLabel = true;

             series = chart.SeriesCollection.AddSeries();
             series.ChartType = ChartType.Line;
             series.Add(new double[] {41, 7, 5, 45, 13, 10, 21, 13, 18, 9});

             var xseries = chart.XValues.AddXSeries();
             xseries.Add("A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N");

             chart.XAxis.MajorTickMark = TickMarkType.Outside;
             chart.XAxis.Title.Caption = "X-Axis";

             chart.YAxis.MajorTickMark = TickMarkType.Outside;
             chart.YAxis.HasMajorGridlines = true;

             chart.PlotArea.LineFormat.Color = Colors.DarkGoldenrod;
             chart.PlotArea.LineFormat.Width = 1;

             document.LastSection.Add(chart);
         }
    }
}