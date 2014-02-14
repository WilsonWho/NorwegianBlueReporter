using System;
using System.Collections.Generic;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace LaserPrinter.Graphs
{
    public class LineStackedGraph : Graph
    {
        private readonly List<Tuple<string, List<double>>> _parameter;

        public LineStackedGraph(string name, bool hasLegend, LegendPositionEnum legendPosition, bool hasDataLabel, List<Tuple<string, List<double>>> parameter)
            : base(name, hasLegend, legendPosition, hasDataLabel)
        {
            _parameter = parameter;
        }

        public override void Draw(Document document)
        {
            var chart = SetUp(ChartType.Line, document);

            foreach (var p in _parameter)
            {
                Series series = chart.SeriesCollection.AddSeries();
                series.Name = p.Item1;
                series.Add(p.Item2.ToArray());
            }

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