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
            var myModel = new PlotModel("Example 1");

            var lineSeries = new LineSeries
                {
                    Points = new List<IDataPoint>
                        {
                            new DataPoint(1, 4),
                            new DataPoint(2, 2),
                            new DataPoint(3, 2),
                            new DataPoint(4, 1),
                            new DataPoint(5, 6),
                            new DataPoint(6, 2),
                            new DataPoint(7, 3)
                        }
                };

            myModel.Series.Add(lineSeries);
        }
    }
}