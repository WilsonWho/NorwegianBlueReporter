using System.ComponentModel;
using LaserOptics;
using LaserPrinter.Graphs;
using LaserPrinter.Graphs.MigraDoc;
using LaserPrinter.Graphs.OxyPlot;

namespace LaserPrinter
{
    public static class GraphFactory
    {
        // Grab plot tool from YAML file
        private static TargetLibrary GetTargetLibrary()
        {
            return TargetLibrary.OxyPlot; // Need to actually fetch from YAML
        }

        public static Graph CreateGraph(GraphData graphData)
        {
            var targetLibrary = GetTargetLibrary();

            switch (targetLibrary)
            {
                case TargetLibrary.MigraDoc:
                    return CreateMigraDocGraph(graphData);
                case TargetLibrary.OxyPlot:
                    return CreateOxyPlotGraph(graphData);
                default:
                    throw new InvalidEnumArgumentException("Graph tool is not supported ...");
            }
        }

        private static Graph CreateMigraDocGraph(GraphData graphData)
        {
            switch (graphData.GraphType)
            {
                case GraphType.Line:
                case GraphType.LineStacked:
                case GraphType.Column:
                case GraphType.ColumnStacked:
                    return new MigraDocSeriesGraph(graphData);
                case GraphType.ColorTable:
                    return new MigraDocColorTableGraph(graphData);
                default:
                    throw new InvalidEnumArgumentException("Graph tool is not supported ...");
            }
        }

        private static Graph CreateOxyPlotGraph(GraphData graphData)
        {
            switch (graphData.GraphType)
            {
                case GraphType.Line:
                case GraphType.LineStacked:
                    return new OxyPlotSeriesGraph(graphData);
                case GraphType.Column:
                case GraphType.ColumnStacked:
                    return new MigraDocSeriesGraph(graphData);
                case GraphType.ColorTable:
                    return new OxyPlotSeriesGraph(graphData);
                default:
                    throw new InvalidEnumArgumentException("Graph tool is not supported ...");
            }
        }
    }
}