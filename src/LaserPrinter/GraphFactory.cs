using System.ComponentModel;
using LaserPrinter.Graphs;
using LaserOptics;

namespace LaserPrinter
{
    public static class GraphFactory
    {
        public static Graph CreateGraph(GraphData graphData)
        {
            Graph graph;

            switch (graphData.GraphType)
            {
                case GraphType.Line:
                    graph = new LineGraph(graphData);
                    break;
                case GraphType.LineStacked:
                    graph = new LineStackedGraph(graphData);
                    break;
                case GraphType.Column:
                    graph = new ColumnGraph(graphData);
                    break;
                case GraphType.ColumnStacked:
                    graph = new ColumnStackedGraph(graphData);
                    break;
                case GraphType.Pie:
                    graph = new PieGraph(graphData);
                    break;
                case GraphType.ExplodedPie:
                    graph = new ExplodedPieGraph(graphData);
                    break;
                case GraphType.ColorTableGraph:
                    graph = new ColorTableGraph(graphData);
                    break;
                default:
                    throw new InvalidEnumArgumentException("Unsupported graph type ...");
            }

            return graph;
        }
    }
}