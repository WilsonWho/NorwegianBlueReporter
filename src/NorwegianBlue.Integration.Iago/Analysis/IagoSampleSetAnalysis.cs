using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using NorwegianBlue.Analysis;
using NorwegianBlue.Analysis.Samples;
using NorwegianBlue.IagoIntegration.Samples;
using NorwegianBlue.Util.Configuration;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace NorwegianBlue.IagoIntegration.Analysis
{
    public class IagoSampleSetAnalysis
    {
        private readonly List<object> _statsToLineGraph;
        
        public IagoSampleSetAnalysis()
        {
            Dictionary<object, object> configuration = YamlParser.GetConfiguration();
            _statsToLineGraph = (List<object>)configuration["Graphs"];
        }
               
        public void IagoRequestLatencySummary(IagoSampleSet sampleSet)
        {
            var averages = sampleSet.AnalysisScratchPad.Averages;
            var stdDeviations = sampleSet.AnalysisScratchPad.StdDeviations;

            var graphs = new List<PlotModel>();
            var notes = new StringBuilder();

            foreach (var o in _statsToLineGraph)
            {
                var graphSpec = (Dictionary<object, object>) o;
                List<object> statSeriesObjects;

                if (graphSpec.ContainsKey("Statistics"))
                {
                    statSeriesObjects = (List<object>)graphSpec["Statistics"];
                }
                else
                {
                    statSeriesObjects = new List<object> {graphSpec["Statistic"]};
                }

                if (0 == statSeriesObjects.Count)
                {
                    throw new ArgumentException("No statistic specified for graph!");
                }

                var statSeries = statSeriesObjects.Select(s => s.ToString()).ToList();

                var includeAverage = (graphSpec["IncludeAverage"].ToString() == "true");
                var includeStdDev = (graphSpec["IncludeStdDeviation"].ToString() == "true");

                if (includeStdDev && !includeAverage)
                {
                    throw new ArgumentException("Can't include add std deviation to graph with out the average.");
                }

                var graph = new PlotModel {Title = graphSpec["Title"].ToString()};
                var xAxis = new DateTimeAxis
                    {
                        Position = AxisPosition.Bottom,
                        Minimum = DateTimeAxis.ToDouble(sampleSet.StartTime),
                        Maximum = DateTimeAxis.ToDouble(sampleSet.EndTime)
                    };
                graph.Axes.Add(xAxis);

                var yAxis = new LinearAxis { Position = AxisPosition.Left};
                graph.Axes.Add(yAxis);

                foreach (var statistic in statSeries)
                {
                    var lineSeries = new LineSeries 
                        {
                            Title = statistic,
                            LineLegendPosition = LineLegendPosition.End
                        };

                    foreach (var sample in sampleSet)
                    {
                        var timeStamp = DateTimeAxis.ToDouble(sample.TimeStamp);
                        double val;

                        if (sample.ContainsKey(statistic))
                        {
                            val = sample[statistic];
                        }
                        else
                        {
                            val = lineSeries.Points.Count > 0 ? lineSeries.Points[lineSeries.Points.Count - 1].Y : 0;
                            notes.AppendFormat("{0} Missing value for {1}\n", sample.TimeStamp, statistic);
                        }
                        lineSeries.Points.Add(new DataPoint(timeStamp, val));                               
                    }
                    graph.Series.Add(lineSeries);

                    if (includeAverage)
                    {
                        var average = averages[statistic];
                        var avgSeries = new LineSeries
                            {
                                Title = statistic + " average",
                                LineLegendPosition = LineLegendPosition.End
                            };


                        var stdDev = stdDeviations[statistic];
                        var stdDevAreaSeries = new AreaSeries
                            {
                                Title = statistic + " std dev range",
                                LineLegendPosition = LineLegendPosition.End
                            };

                        foreach (var dataPoint in lineSeries.Points)
                        {
                            var timeStamp = dataPoint.X;
                            avgSeries.Points.Add(new DataPoint(timeStamp, average));

                            stdDevAreaSeries.Points.Add( new DataPoint(timeStamp, average + stdDev));
                            stdDevAreaSeries.Points2.Add( new DataPoint(timeStamp, average - stdDev));
                        }

                        graph.Series.Add(avgSeries);

                        if (includeStdDev)
                        {
                            graph.Series.Add(stdDevAreaSeries);
                        }
                    }
                }
                graphs.Add(graph);
            }
            
            var analysisSummary = new StringBuilder(
                @"# Average Request Latency

The following graphs the average client request statistics for each minute of the test duration.
"
                );

            if (notes.Length > 0)
            {
                analysisSummary.Append(
                    @"*Notes*:
Missing data is replaced either with a 0 when there is no preceeding data or the previous value.

*Time stamps with Missing Data*
"
                    );
                analysisSummary.Append(notes);

            }

            var note = new AnalysisNote("Request Latencies", analysisSummary.ToString(), AnalysisNote.AnalysisNoteType.Summary, AnalysisNote.AnalysisNotePriorities.Important, graphs);
            sampleSet.AddAnalysisNote(note);
        }
    }
}
