using System;
using System.ComponentModel;
using LaserOptics.Common;
using LaserPrinter.Graphs;
using LaserPrinter.Graphs.MigraDoc;
using LaserPrinter.Graphs.OxyPlot;

namespace LaserPrinter
{
    public static class GraphFactory
    {
        private static TargetLibrary? _targetLibrary;

        public static Graph CreateGraph(GraphData graphData)
        {
            if (graphData.GraphType == GraphType.ColorTable)
            {
                return new MigraDocColorTableGraph(graphData);
            }

            switch (_targetLibrary)
            {
                case TargetLibrary.MigraDoc:
                    return CreateMigraDocGraph(graphData);
                case TargetLibrary.OxyPlot: 
                default:
                    return CreateOxyPlotGraph(graphData);
            }
        }

        public static void SetTargetLibrary(string graphType)
        {
            _targetLibrary = (TargetLibrary) Enum.Parse(typeof (TargetLibrary), graphType);
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