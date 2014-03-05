using System;
using System.Collections.Generic;
using System.ComponentModel;
using LaserOptics.Common;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using LaserOptics;

namespace LaserPrinter.Graphs.MigraDoc
{
    public abstract class MigraDocGraph : Graph
    {
        private readonly List<Tuple<char, string>> _legendIndex;

        protected GraphData GraphData;

        protected MigraDocGraph(GraphData graphData)
        {
            _legendIndex = new List<Tuple<char, string>>();
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

            var title = chart.TopArea.AddParagraph(GraphData.Title);
            title.Format.Font.Bold = true;

            LabelXAxis(chart);

            if (GraphData.GraphType != GraphType.ColorTable)
            {
                AssociateAllSeriesData(chart);
                GenerateLegendIndex(chart);
            }
            
            return chart;
        }

        protected override void SetGlobalChartOptions(Chart chart)
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

        private void LabelXAxis(Chart chart)
        {
            XSeries xseries = chart.XValues.AddXSeries();

            var length = GraphData.Labels.Count;
            for (int i = 0; i < length; i++)
            {
                if (i == 0)
                {
                    xseries.Add(GraphData.Labels[i]);
                }
                else if (i == length - 1)
                {
                    xseries.Add(GraphData.Labels[length - 1]);
                }
                else
                {
                    xseries.Add(string.Empty);
                }
            }
        }

        private void AssociateAllSeriesData(Chart chart)
        {
            char seriesShortName = 'A';

            foreach (var seriesData in GraphData.SeriesData)
            {
                var series = chart.SeriesCollection.AddSeries();
                _legendIndex.Add(new Tuple<char, string>(seriesShortName, seriesData.Name));
                series.Name = seriesShortName.ToString();
                series.Add(seriesData.Data.ToArray());

                series.MarkerStyle = MarkerStyle.None;

                seriesShortName++;
                if (seriesShortName > 'Z')
                {
                    throw new ArgumentException("More than 26 series supplied!");
                }
            }
        }

        private void GenerateLegendIndex(Chart chart)
        {
            var bottomPar = chart.BottomArea.AddParagraph();
            bottomPar.AddFormattedText("Key for the Key", TextFormat.Bold);

            bottomPar = chart.BottomArea.AddParagraph();

            foreach (var tuple in _legendIndex)
            {
                bottomPar.AddText(string.Format("{0} is {1}\n", tuple.Item1, tuple.Item2));
            }
        }
    }
}