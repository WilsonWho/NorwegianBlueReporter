using System.ComponentModel;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace LaserPrinter.Graphs
{
    public abstract class Graph
    {
        public enum LegendPositionEnum
        {
            Left,
            Right,
            Footer
        }

        public string Name { get; private set; }
        public bool HasLegend { get; private set; }
        public LegendPositionEnum LegendPosition { get; private set; }
        public bool HasDataLabel { get; private set; }

        protected Graph(string name, bool hasLegend, LegendPositionEnum legendPosition, bool hasDataLabel)
        {
            Name = name;
            HasLegend = hasLegend;
            LegendPosition = legendPosition;
            HasDataLabel = hasDataLabel;
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

            return chart;
        }

        protected void SetGlobalChartOptions(Chart chart)
        {
            if (HasDataLabel)
            {
                chart.DataLabel.Type = DataLabelType.Value;
                chart.DataLabel.Position = DataLabelPosition.Center;
            }

            if (HasLegend)
            {
                switch (LegendPosition)
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