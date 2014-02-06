using System;
using System.Collections.Generic;
using System.Linq;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using StatsReader;

namespace LaserPrinter
{
    public class GraphManager : IGraphManager
    {
        // TODO: Needs to take in set of data to graph
        public void DefineComboGraph(Document document)
        {
            // Example header details
            Paragraph paragraph = document.LastSection.AddParagraph("Chart Overview", "Heading1");
            paragraph.AddBookmark("Charts");

            document.LastSection.AddParagraph("Sample Chart", "Heading2");

            ComboGraphExample(document);
        }

        public void DefineColumnStackedGraph(Document document, List<SeriesData> seriesDataList)
        {
            // TODO: Add header details

            ColumnStackedChartExample(document, seriesDataList);
        }

        private void ComboGraphExample(Document document)
        {
            var chart = new Chart
                {
                    Left = 0,
                    Width = Unit.FromCentimeter(16),
                    Height = Unit.FromCentimeter(12)
                };

            var seriesL1 = chart.SeriesCollection.AddSeries();
            seriesL1.Add(new double[] { 4, 17, 5, 25, 13, 6, 42, 31, 11, 28 });
            seriesL1.LineFormat.Color = Colors.IndianRed;
            seriesL1.LineFormat.Width = 2;
            seriesL1.HasDataLabel = true;
            seriesL1.Name = "The Other Best Line";

            var seriesL2 = chart.SeriesCollection.AddSeries();
            seriesL2.Add(new double[] { 41, 7, 5, 45, 13, 10, 21, 13, 18, 9 });
            seriesL2.LineFormat.Color = Colors.Moccasin;
            seriesL2.LineFormat.Width = 2;
            seriesL2.HasDataLabel = true;
            seriesL2.Name = "The Best Line";

            var xseries = chart.XValues.AddXSeries();
            xseries.Add("A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N");

            chart.XAxis.MajorTickMark = TickMarkType.Outside;
            chart.XAxis.Title.Caption = "X-Axis";

            chart.YAxis.MajorTickMark = TickMarkType.Outside;
            chart.YAxis.HasMajorGridlines = true;

            chart.PlotArea.LineFormat.Color = Colors.DarkGoldenrod;
            chart.PlotArea.LineFormat.Width = 1;

            chart.FooterArea.AddLegend();

            document.LastSection.Add(chart);
        }

        private void ColumnStackedChartExample(Document document, List<SeriesData> seriesDataList)
        {
            var chart = new Chart
                {
                    Left = ShapePosition.Center,
                    Width = Unit.FromCentimeter(16),
                    Height = Unit.FromCentimeter(12),
                    Type = ChartType.ColumnStacked2D
                };

            

            foreach (var seriesData in seriesDataList)
            {
                Series series = chart.SeriesCollection.AddSeries();
                series.Name = seriesData.Name;
                var values = seriesData.Data.Select(data => data.Item2).Cast<double>().ToList();

                series.Add(values.ToArray());
            }

            //series.Add(new double[] {1, 5, -3, 20, 11});

            //series = chart.SeriesCollection.AddSeries();
            //series.Name = "Series 2";
            //series.Add(new double[] {22, 4, 12, 8, 12});

            //series = chart.SeriesCollection.AddSeries();
            //series.Name = "Series 3";
            //series.Add(new double[] {12, 14, 2, 18, 1});

            //series = chart.SeriesCollection.AddSeries();
            //series.Name = "Series 4";
            //series.Add(new double[] {17, 13, 10, 9, 15});

            chart.XAxis.TickLabels.Format = "00";
            chart.XAxis.MajorTickMark = TickMarkType.Outside;
            chart.XAxis.Title.Caption = "X-Axis";

            chart.YAxis.MajorTickMark = TickMarkType.Outside;
            chart.YAxis.HasMajorGridlines = true;

            chart.PlotArea.LineFormat.Color = Colors.DarkGray;
            chart.PlotArea.LineFormat.Width = 1;
            chart.PlotArea.LineFormat.Visible = true;

            chart.RightArea.AddLegend();

            chart.DataLabel.Type = DataLabelType.Value;
            chart.DataLabel.Position = DataLabelPosition.Center;

            document.LastSection.Add(chart);
        }

        private void BarChartExample(Document document)
        {
            var chart = new Chart
                {
                    Left = ShapePosition.Center,
                    Width = Unit.FromCentimeter(16),
                    Height = Unit.FromCentimeter(12),
                    Type = ChartType.ColumnStacked2D
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
                    Left = ShapePosition.Center,
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

            XSeries xseries = chart.XValues.AddXSeries();
            xseries.Add("2", "3", "4", "5", "6");

            chart.XAxis.Title.Caption = "X-Axis";

            chart.YAxis.MajorTickMark = TickMarkType.Outside;
            chart.YAxis.HasMajorGridlines = true;

            chart.PlotArea.LineFormat.Color = Colors.DarkGray;
            chart.PlotArea.LineFormat.Width = 2;
            chart.PlotArea.LineFormat.Visible = true;

            chart.DataLabel.Type = DataLabelType.Value;
            chart.DataLabel.Position = DataLabelPosition.InsideBase;

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

            //chart.XAxis.TickLabels.Format = "00";
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