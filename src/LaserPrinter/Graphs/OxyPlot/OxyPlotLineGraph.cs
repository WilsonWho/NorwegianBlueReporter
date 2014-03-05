using System;
using System.Collections.Generic;
using System.Linq;
using LaserOptics;
using MigraDoc.DocumentObjectModel;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace LaserPrinter.Graphs.OxyPlot
{
    public class OxyPlotLineGraph : OxyPlotGraph
    {
        public OxyPlotLineGraph(GraphData graphData) : base(graphData)
        {
        }

        public override void Draw(Document document)
        {
            var plotModel = SetUp(document);
            SetAxes(plotModel);
            AddData(plotModel);

            var fileName = ExportPng(plotModel);
            SaveToMigraDocPdf(fileName, document);
        }

        protected override void SetAxes(PlotModel plotModel)
        {
            plotModel.Axes.Add(new LinearAxis(AxisPosition.Left)
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                TickStyle = TickStyle.Outside
            });

            var offset = new TimeSpan(0, 0, 0, 1);
            var min = Convert.ToDateTime(GraphData.Labels[0]) - offset;
            var max = Convert.ToDateTime(GraphData.Labels[GraphData.Labels.Count - 1]) + offset;

            var duration = max - min;

            plotModel.Axes.Add(new DateTimeAxis(AxisPosition.Bottom, min, max, "Time", "yy-MM-dd hh:mm:ss", DateTimeIntervalType.Milliseconds)
            {
                Angle = 45,
                ShowMinorTicks = false,
                MajorStep = (duration.TotalMilliseconds / (24 * 60 * 60 * 1000)) / 2.5,
                TickStyle = TickStyle.Inside
            });
        }

        protected override void AddData(PlotModel plotModel)
        {
            foreach (SeriesData seriesData in GraphData.SeriesData)
            {
                var lineSeries = new LineSeries(seriesData.Name) { DataFieldX = "X", DataFieldY = "Y"};

                var lineData = new List<LineData>();
                for (int i = 0; i < seriesData.Data.Count; i++)
                {
                    var dateTime = Convert.ToDateTime(GraphData.Labels[i]);
                    var dateTimeAxisDouble = DateTimeAxis.ToDouble(dateTime);

                    lineData.Add(new LineData { X = dateTimeAxisDouble, Y = seriesData.Data[i] });
                }

                lineSeries.ItemsSource = lineData;
                plotModel.Series.Add(lineSeries);
            }
        }
    }

    internal class LineData
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}