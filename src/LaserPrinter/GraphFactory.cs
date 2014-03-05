using System.ComponentModel;
using LaserOptics;
using LaserOptics.Common;
using LaserPrinter.Graphs;
using LaserPrinter.Graphs.MigraDoc;
using LaserPrinter.Graphs.OxyPlot;

namespace LaserPrinter
{
    public static class GraphFactory
    {
        private static TargetLibrary? _targetLibrary;

        // Grab plot tool from YAML file
        public static void SetTargetLibrary(TargetLibrary targetLibrary)
        {
            _targetLibrary = targetLibrary; // Need to actually fetch from YAML
        }

        public static Graph CreateGraph(GraphData graphData)
        {
            if (_targetLibrary == null)
            {
                throw new InvalidEnumArgumentException("Enum has not been defined!");
            }

            if (graphData.GraphType == GraphType.ColorTable)
            {
                return new MigraDocColorTableGraph(graphData);
            }

            switch (_targetLibrary)
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
                    return new MigraDocLineGraph(graphData);
                case GraphType.Column:
                case GraphType.ColumnStacked:
                    return new MigraDocColumnGraph(graphData);
                default:
                    throw new InvalidEnumArgumentException("Graph type is not supported ...");
            }
        }

        private static Graph CreateOxyPlotGraph(GraphData graphData)
        {
            switch (graphData.GraphType)
            {
                case GraphType.Line:
                case GraphType.LineStacked:
                    return new OxyPlotLineGraph(graphData);
                case GraphType.Column:
                case GraphType.ColumnStacked:
                    return new OxyPlotColumnGraph(graphData);
                case GraphType.Pie:
                case GraphType.ExplodedPie:
                    return new OxyPlotPieGraph(graphData);
                default:
                    throw new InvalidEnumArgumentException("Graph type is not supported ...");
            }
        }
    }
}