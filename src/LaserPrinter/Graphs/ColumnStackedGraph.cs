using System.Collections.Generic;
using System.Linq;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using StatsReader;

namespace LaserPrinter.Graphs
{
    public class ColumnStackedGraph : Graph
    {
        private readonly List<SeriesData> _seriesDataList; 

        public ColumnStackedGraph(string name, bool hasLegend, LegendPositionEnum legendPosition, bool hasDataLabel, List<SeriesData> seriesDataList)
            : base(name, hasLegend, legendPosition, hasDataLabel)
        {
            _seriesDataList = seriesDataList;
        }

        public override void Draw(Document document)
        {
            var chart = SetUp(ChartType.ColumnStacked2D, document);

            foreach (var seriesData in _seriesDataList)
            {
                Series series = chart.SeriesCollection.AddSeries();
                series.Name = seriesData.Name;
                var values = seriesData.Data.Select(data => data.Item2).Cast<double>().ToList();

                series.Add(values.ToArray());
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