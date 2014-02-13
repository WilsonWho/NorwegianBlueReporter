using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace LaserPrinter
{
    public class ColumnGraph : Graph
    {
        private List<Tuple<string, double>>  _data;

        public ColumnGraph(string name, bool hasLegend, LegendPositionEnum legendPosition, bool hasDataLabel, List<Tuple<string, double>> data)
            : base(name, hasLegend, legendPosition, hasDataLabel)
        {
            _data = data;
        }

        public override void Draw(Document document)
        {
            var chart = SetUp(ChartType.Column2D, document);

            Series series = chart.SeriesCollection.AddSeries();
            XSeries xseries = chart.XValues.AddXSeries();

            foreach (var tuple in _data)
            {
                xseries.Add(tuple.Item1);
                series.Add(tuple.Item2);
            }

            chart.XAxis.TickLabels.Format = "00";
            chart.XAxis.MajorTickMark = TickMarkType.Outside;

            chart.YAxis.MajorTickMark = TickMarkType.Outside;
            chart.YAxis.HasMajorGridlines = true;

            chart.PlotArea.LineFormat.Color = Colors.DarkGray;
            chart.PlotArea.LineFormat.Width = 1;
            chart.PlotArea.LineFormat.Visible = true;
            chart.HeaderArea.AddParagraph(Name);
            
            SetGlobalChartOptions(chart);

            document.LastSection.Add(chart);
        }
    }
}