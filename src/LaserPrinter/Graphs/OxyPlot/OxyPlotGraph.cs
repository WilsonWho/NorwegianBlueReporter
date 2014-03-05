using System;
using System.Drawing;
using System.IO;
using LaserOptics;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using OxyPlot;
using OxyPlot.WindowsForms;

namespace LaserPrinter.Graphs.OxyPlot
{
    public abstract class OxyPlotGraph : Graph
    {
        protected GraphData GraphData;

        protected abstract void SetAxes(PlotModel plotModel);
        protected abstract void AddData(PlotModel plotModel);

        protected OxyPlotGraph(GraphData graphData)
        {
            GraphData = graphData;
        }

        protected PlotModel SetUp(Document document)
        {
            return new PlotModel(GraphData.Title)
                {
                    LegendPlacement = LegendPlacement.Outside,
                    LegendPosition = LegendPosition.BottomCenter,
                    LegendOrientation = LegendOrientation.Horizontal,
                    LegendPadding = 100
                };
        }

        protected override void SetGlobalChartOptions(Chart chart)
        {
            throw new NotImplementedException();
        }

        protected string ExportPng(PlotModel plotModel)
        {
            var tmp = Path.GetTempFileName();
            PngExporter.Export(plotModel, tmp, 800, 800, Brushes.White);

            return tmp;
        }

        protected void SaveToMigraDocPdf(string fileName, Document document)
        {
            var image = document.LastSection.AddImage(fileName);
            image.Width = Unit.FromPoint(400);
        }
    }
}