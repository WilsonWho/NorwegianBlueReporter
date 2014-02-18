using System.ComponentModel;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using StatsReader;

namespace LaserPrinter.Graphs
{
    public abstract class Graph
    {
        protected GraphData GraphData; 

        protected Graph(GraphData graphData)
        {
            GraphData = graphData;
        }

        protected Chart SetUp(ChartType chartType, Document document)
        {
            var chart = new Chart
            {
                Left = ShapePosition.Center,
                Width = Unit.FromCentimeter(16),
                Height = Unit.FromCentimeter(12),
                Type = chartType
            };

            XSeries xseries = chart.XValues.AddXSeries();
            xseries.Add(GraphData.Labels.ToArray());

            return chart;
        }

        protected void SetGlobalChartOptions(Chart chart)
        {
            if (GraphData.HasDataLabel)
            {
                chart.DataLabel.Type = DataLabelType.Value;
                chart.DataLabel.Position = DataLabelPosition.Center;
            }

            if (GraphData.HasLegend)
            {
                switch (GraphData.LegendPosition)
                {
                    case LegendPositionEnum.Footer:
                        chart.FooterArea.AddLegend();
                        break;
                    case LegendPositionEnum.Right:
                        chart.RightArea.AddLegend();
                        break;
                    case LegendPositionEnum.Left:
                        chart.LeftArea.AddLegend();
                        break;
                    default:
                        throw new InvalidEnumArgumentException("Unknown legend position ...");
                }
            }
        }

        public abstract void Draw(Document document);
    }
}