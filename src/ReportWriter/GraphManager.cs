using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace LaserPrinter
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

            ComboGraphExample(document);
            //BarChartExample(document);
            //BarStackedChartExample(document);
            //AreaChartExample(document);
            //PieChartExample(document);
            //ExplodedPieChartExample(document);
        }

        private void ComboGraphExample(Document document)
        {
            var chart = new Chart
                {
                    Left = 0,
                    Width = Unit.FromCentimeter(16),
                    Height = Unit.FromCentimeter(12)
                };

            var series = chart.SeriesCollection.AddSeries();
            series.ChartType = ChartType.Column2D;
            series.Add(new double[] { 1, 17, 45, 5, 3, 20, 11, 23, 8, 19 });
            series.HasDataLabel = true;

            series = chart.SeriesCollection.AddSeries();
            series.ChartType = ChartType.Column2D;
            series.Add(new double[] { 4, 17, 5, 25, 13, 6, 42, 31, 11, 28 });

            //series = chart.SeriesCollection.AddSeries();
            //series.ChartType = ChartType.Line;
            //series.Add(new double[] { 41, 7, 5, 45, 13, 10, 21, 13, 18, 9 });

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

        private void BarChartExample(Document document)
        {
            var chart = new Chart
                {
                    Left = 0,
                    Width = Unit.FromCentimeter(16),
                    Height = Unit.FromCentimeter(12),
                    Type = ChartType.Bar2D
                };

            Series series = chart.SeriesCollection.AddSeries();
            series.Name = "Series 1";
            series.Add(new double[] { 1, 5, -3, 20, 11 });

            series = chart.SeriesCollection.AddSeries();
            series.Name = "Series 2";
            series.Add(new double[] { 22, 4, 12, 8, 12 });

            series = chart.SeriesCollection.AddSeries();
            series.Name = "Series 3";
            series.Add(new double[] { 12, 14, 2, 18, 1 });

            series = chart.SeriesCollection.AddSeries();
            series.Name = "Series 4";
            series.Add(new double[] { 17, 13, 10, 9, 15 });

            chart.XAxis.MajorTickMark = TickMarkType.Outside;
            chart.XAxis.Title.Caption = "X-Axis";

            chart.YAxis.MajorTickMark = TickMarkType.Outside;
            chart.YAxis.HasMajorGridlines = true;

            chart.PlotArea.LineFormat.Color = Colors.DarkGray;
            chart.PlotArea.LineFormat.Width = 1;
            chart.PlotArea.LineFormat.Visible = true;

            chart.DataLabel.Type = DataLabelType.Value;
            chart.DataLabel.Position = DataLabelPosition.InsideEnd;

            document.LastSection.Add(chart);
        }

        private void BarStackedChartExample(Document document)
        {
            var chart = new Chart
                {
                    Left = 0,
                    Width = Unit.FromCentimeter(16),
                    Height = Unit.FromCentimeter(12),
                    Type = ChartType.BarStacked2D
                };

            Series series = chart.SeriesCollection.AddSeries();
            series.Name = "Series 1";
            series.Add(new double[] { 1, 5, -3, 20, 11 });

            series = chart.SeriesCollection.AddSeries();
            series.Name = "Series 2";
            series.Add(new double[] { 22, 4, 12, 8, 12 });

            series = chart.SeriesCollection.AddSeries();
            series.Name = "Series 3";
            series.Add(new double[] { 12, 14, 2, 18, 1 });

            series = chart.SeriesCollection.AddSeries();
            series.Name = "Series 4";
            series.Add(new double[] { 17, 13, 10, 9, 15 });

            chart.XAxis.MajorTickMark = TickMarkType.Outside;
            chart.XAxis.Title.Caption = "X-Axis";

            chart.YAxis.MajorTickMark = TickMarkType.Outside;
            chart.YAxis.HasMajorGridlines = true;

            chart.PlotArea.LineFormat.Color = Colors.DarkGray;
            chart.PlotArea.LineFormat.Width = 1;
            chart.PlotArea.LineFormat.Visible = true;

            chart.DataLabel.Type = DataLabelType.Value;
            chart.DataLabel.Position = DataLabelPosition.Center;

            document.LastSection.Add(chart);
        }

        private void AreaChartExample(Document document)
        {
            var chart = new Chart
                {
                    Left = 0,
                    Width = Unit.FromCentimeter(16),
                    Height = Unit.FromCentimeter(12),
                    Type = ChartType.Area2D
                };

            Series series = chart.SeriesCollection.AddSeries();
            series.Add(new double[] { 31, 9, 15, 28, 13 });

            series = chart.SeriesCollection.AddSeries();
            series.Add(new double[] { 22, 7, 12, 21, 12 });

            series = chart.SeriesCollection.AddSeries();
            series.Add(new double[] { 16, 5, 3, 20, 11 });

            chart.XAxis.TickLabels.Format = "00";
            chart.XAxis.MajorTickMark = TickMarkType.Outside;
            chart.XAxis.Title.Caption = "X-Axis";

            chart.YAxis.MajorTickMark = TickMarkType.Outside;
            chart.YAxis.HasMajorGridlines = true;

            chart.PlotArea.LineFormat.Color = Colors.DarkGray;
            chart.PlotArea.LineFormat.Width = 1;
            chart.PlotArea.LineFormat.Visible = true;

            document.LastSection.Add(chart);
        }

        private void PieChartExample(Document document)
        {
            var chart = new Chart
                {
                    Left = 0,
                    Width = Unit.FromCentimeter(16),
                    Height = Unit.FromCentimeter(12),
                    Type = ChartType.Pie2D
                };

            var series = chart.SeriesCollection.AddSeries();
            series.Add(new double[] { 1, 5, 11, -3, 20 });

            var xseries = chart.XValues.AddXSeries();
            xseries.Add("Production", "Lab", "Licenses", "Taxes", "Insurances");
            chart.DataLabel.Type = DataLabelType.Percent;
            chart.DataLabel.Position = DataLabelPosition.OutsideEnd;
            
            document.LastSection.Add(chart);
        }

        private void ExplodedPieChartExample(Document document)
        {
            var chart = new Chart
            {
                Left = 0,
                Width = Unit.FromCentimeter(16),
                Height = Unit.FromCentimeter(12),
                Type = ChartType.PieExploded2D
            };

            var series = chart.SeriesCollection.AddSeries();
            series.Add(new double[] { 1, 17, 45, 5, 3, 20, 11, 23, 8, 19, 34, 56, 23, 45 });

            chart.DataLabel.Type = DataLabelType.Percent;
            chart.DataLabel.Position = DataLabelPosition.Center;

            document.LastSection.Add(chart);
        }
    }
}