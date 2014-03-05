using System;
using LaserOptics;
using MigraDoc.DocumentObjectModel;
using OxyPlot;
using OxyPlot.Series;

namespace LaserPrinter.Graphs.OxyPlot
{
    public class OxyPlotPieGraph : OxyPlotGraph
    {
        public OxyPlotPieGraph(GraphData graphData) : base(graphData)
        {
            if (graphData.SeriesData.Count != 1)
            {
                throw new ArgumentException("Must only have 1 series!");
            }
        }

        public override void Draw(Document document)
        {
            var plotModel = SetUp(document);
            AddData(plotModel);

            var fileName = ExportPng(plotModel);
            SaveToMigraDocPdf(fileName, document);
        }

        protected override void SetAxes(PlotModel plotModel)
        {
            throw new NotImplementedException();
        }

        protected override void AddData(PlotModel plotModel)
        {
            var pieSeries = new PieSeries();
            var labels = GraphData.Labels;
            var seriesData = GraphData.SeriesData[0].Data;

            for (int index = 0; index < labels.Count; index++)
            {
                pieSeries.Slices.Add(new PieSlice {Label = labels[index], Value = seriesData[index]});
            }

            pieSeries.InnerDiameter = 0;
            pieSeries.ExplodedDistance = 0.0;
            pieSeries.Stroke = OxyColors.White;
            pieSeries.StrokeThickness = 2.0;
            pieSeries.InsideLabelPosition = 0.8;
            pieSeries.AngleSpan = 360;
            pieSeries.StartAngle = 0;

            plotModel.Series.Add(pieSeries);
        }
    }
}