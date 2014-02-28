using System.Collections.Generic;
using System.IO;
using LaserOptics;
using MigraDoc.DocumentObjectModel;
using OxyPlot;
using OxyPlot.Series;

namespace LaserPrinter.Graphs.OxyPlot
{
    public class OxyPlotSeriesGraph : OxyPlotGraph
    {
        public OxyPlotSeriesGraph(GraphData graphData) : base(graphData)
        {
        }

        public override void Draw(Document document)
        {
            var plotModel = SetUp(document);
            var tmp = Path.GetTempFileName();

            ExportPng(tmp, plotModel);

            var image = document.LastSection.AddImage(tmp);
            image.Width = Unit.FromPoint(400);
        }
    }
}