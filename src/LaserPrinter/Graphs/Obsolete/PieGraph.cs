using System;
using LaserOptics.Common;
using LaserPrinter.Graphs.MigraDoc;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace LaserPrinter.Graphs.Obsolete
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