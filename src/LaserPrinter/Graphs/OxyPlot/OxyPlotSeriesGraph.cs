using System.Collections.Generic;
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

            ExportPng(@"Wassup.png", plotModel);
        }
    }
}