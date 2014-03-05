using LaserOptics.Common;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace LaserPrinter.Graphs
{
    public class ExplodedPieGraph : Graph
    {
        public ExplodedPieGraph(GraphData graphData) : base(graphData)
        {
        }

        public override void Draw(Document document)
        {
            var chart = SetUp(ChartType.PieExploded2D, document);

            foreach (var seriesData in GraphData.SeriesData)
            {
                Series series = chart.SeriesCollection.AddSeries();
                series.Name = seriesData.Name;
                series.Add(seriesData.Data.ToArray());
            }

            SetGlobalChartOptions(chart);

            document.LastSection.Add(chart);
        }
    }
}