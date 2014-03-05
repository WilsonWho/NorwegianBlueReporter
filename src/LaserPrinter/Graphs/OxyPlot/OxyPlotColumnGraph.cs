using LaserOptics;
using MigraDoc.DocumentObjectModel;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace LaserPrinter.Graphs.OxyPlot
{
    public class OxyPlotColumnGraph : OxyPlotGraph
    {
        public OxyPlotColumnGraph(GraphData graphData) : base(graphData)
        {
        }

        public override void Draw(Document document)
        {
            var plotModel = SetUp(document);
            SetAxes(plotModel);
            AddData(plotModel);

            var fileName = ExportPng(plotModel);
            SaveToMigraDocPdf(fileName, document);
        }

        protected override void SetAxes(PlotModel plotModel)
        {
            var length = GraphData.Labels.Count;

            var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom, Angle = 90};

            var i = 0;
            while (i < length)
            {
                categoryAxis.Labels.Add(GraphData.Labels[i]);
                i++;
            }

            var valueAxis = new LinearAxis(AxisPosition.Left) { MinimumPadding = 0, MaximumPadding = 0.06, AbsoluteMinimum = 0 };

            plotModel.Axes.Add(categoryAxis);
            plotModel.Axes.Add(valueAxis);
        }

        protected override void AddData(PlotModel plotModel)
        {
            foreach (var seriesData in GraphData.SeriesData)
            {
                var columnSeries = new ColumnSeries {Title = seriesData.Name};

                for (int i = 0; i < seriesData.Data.Count; i++)
                {
                    double value = seriesData.Data[i];
                    columnSeries.Items.Add(new ColumnItem(value, i));
                }

                plotModel.Series.Add(columnSeries);
            }
        }
    }
}