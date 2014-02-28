using System;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using LaserOptics;

namespace LaserPrinter.Graphs.MigraDoc
{
    public class PieGraph : MigraDocGraph
    {
        public PieGraph(GraphData graphData) : base(graphData)
        {
            if (graphData.SeriesData.Count != 1)
            {
                throw new ArgumentException("Graph data series count is not equal to 1 ...");
            }
        }

        public override void Draw(Document document)
        {
            var chart = SetUp(ChartType.Pie2D, document);

            var series = chart.SeriesCollection.AddSeries();
            series.Add(GraphData.SeriesData[0].Data.ToArray());

            SetGlobalChartOptions(chart);

            document.LastSection.Add(chart);
        }
    }
}