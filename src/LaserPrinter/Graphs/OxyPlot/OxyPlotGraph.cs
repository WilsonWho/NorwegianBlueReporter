using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using LaserOptics;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace LaserPrinter.Graphs.OxyPlot
{
    public abstract class OxyPlotGraph : Graph
    {
        protected GraphData GraphData;

        protected OxyPlotGraph(GraphData graphData)
        {
            GraphData = graphData;
        }

        protected PlotModel SetUp(Document document)
        {
            var plotModel = new PlotModel(GraphData.Title);

            switch (GraphData.GraphType)
            {
                case GraphType.Line:
                case GraphType.LineStacked:
                    CreateLineSeries(plotModel);
                    break;
                case GraphType.Column:
                case GraphType.ColumnStacked:
                    CreateColumnSeries(plotModel);
                    break;
                default:
                    throw new InvalidEnumArgumentException("Graph type not supported ...");
            }

            return plotModel;
        }

        protected override void SetGlobalChartOptions(Chart chart)
        {
            throw new NotImplementedException();
        }

        protected void ExportPng(string fileName, PlotModel plotModel)
        {
            PngExporter.Export(plotModel, fileName, 600, 400, Brushes.White);
        }

        private void CreateLineSeries(PlotModel plotModel)
        {
            foreach (SeriesData seriesData in GraphData.SeriesData)
            {
                var lineSeries = new LineSeries(seriesData.Name);

                for (int j = 0; j < seriesData.Data.Count; j++)
                {
                    double value = seriesData.Data[j];
                    lineSeries.Points.Add(new DataPoint(j, value));
                }

                plotModel.Series.Add(lineSeries);
            }
        }

        private void CreateColumnSeries(PlotModel plotModel)
        {
            foreach (var seriesData in GraphData.SeriesData)
            {
                var columnSeries = new ColumnSeries();

                for (int i = 0; i < seriesData.Data.Count; i++)
                {
                    double value = seriesData.Data[i];
                    columnSeries.Items.Add(new ColumnItem(value, i));
                }

                plotModel.Series.Add(columnSeries);
            } 
        }
    }
}