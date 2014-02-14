using System.Collections.Generic;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace LaserPrinter.Graphs
{
    public class LineGraph : Graph
    {
        private readonly List<double> _parameter;

        public LineGraph(string name, bool hasLegend, LegendPositionEnum legendPosition, bool hasDataLabel, List<double> parameter) : base(name, hasLegend, legendPosition, hasDataLabel)
        {
            _parameter = parameter;
        }

        public override void Draw(Document document)
        {
            var chart = SetUp(ChartType.Line, document);

            Series series = chart.SeriesCollection.AddSeries();
            series.Name = "Series 1";
            series.Add(_parameter.ToArray());

            XSeries xseries = chart.XValues.AddXSeries();
            xseries.Add("A", "B", "C", "D", "E", "F");

            chart.XAxis.MajorTickMark = TickMarkType.Outside;
            chart.XAxis.Title.Caption = "X-Axis";

            chart.YAxis.MajorTickMark = TickMarkType.Outside;
            chart.YAxis.Title.Caption = "Y-Axis";
            chart.YAxis.HasMajorGridlines = true;

            SetGlobalChartOptions(chart);

            document.LastSection.Add(chart);
        }
    }
}