using System;
using LaserOptics;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace LaserPrinter.Graphs.OxyPlot
{
    public abstract class OxyPlotGraph : Graph
    {
        protected GraphData GraphData;

        protected OxyPlotGraph(GraphData graphData)
        {
            GraphData = graphData;
        }

        protected override Chart SetUp(ChartType chartType, Document document)
        {
            throw new NotImplementedException();
        }

        protected override void SetGlobalChartOptions(Chart chart)
        {
            throw new NotImplementedException();
        }
    }
}