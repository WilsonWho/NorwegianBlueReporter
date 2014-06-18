using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NorwegianBlue.Data.Sample;
using NorwegianBlue.Data.SampleSet;
using NorwegianBlue.Notes.AnalysisNotes;
using OpenCvSharp;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace NorwegianBlue.CommonAnalyzers.Algorithms
{
    public class CommonSampleSetAnalysis
    {
        public void FindAllHeaders(ISampleSetAnalysis<ISampleAnalysis> sampleSet)
        {
            var statsHeaders = new HashSet<String>();
            var nonStatsHeaders = new HashSet<String>();

            foreach (var stat in sampleSet)
            {
                statsHeaders.UnionWith(stat.Keys);
                nonStatsHeaders.UnionWith(stat.NonStats.Keys);
            }

            sampleSet.AnalysisScratchPad.AllStatsHeaders = statsHeaders;
            sampleSet.AnalysisScratchPad.AllNonStatsHeaders = nonStatsHeaders;
        }

        private static void LoopOverStatsAndHeaders(ISampleSetAnalysis<ISampleAnalysis> sampleSet, Dictionary<string, double> values, Func<string, double, double, double> action, Func<string, double> defaultValue)
        {
            foreach (var stat in sampleSet)
            {
                foreach (string key in sampleSet.AnalysisScratchPad.AllStatsHeaders)
                {
                    if (!values.ContainsKey(key))
                    {
                        values[key] = defaultValue(key);
                    }
                    if (stat.ContainsKey(key))
                    {
                        values[key] = action(key, values[key], stat[key]);
                    }
                }
            }
        }

        private static void Apply(Dictionary<string, double> values, Func<double, double> applicant )
        {
            foreach (var key in values.Keys.ToList())
            {
                values[key] = applicant(values[key]);
            }
        }

        public void SummaryStats(ISampleSetAnalysis<ISampleAnalysis> sampleSet)
        {
            var statCount = sampleSet.Count;

            var averages = new Dictionary<string, double>();
            LoopOverStatsAndHeaders(sampleSet, averages,
                                    (key, accumulatedValue, statValue) => accumulatedValue + statValue,
                                    arg => 0d);

            Apply(averages, value => value/statCount);
            sampleSet.AnalysisScratchPad.Averages = averages;

            var stdDeviations = new Dictionary<string, double>();
            // if there is a missing value use the average value for the field as the 'default' value, rather than zero (ie try not to inflate the std deviation.)
            LoopOverStatsAndHeaders(sampleSet, stdDeviations,
                                    (key, accumulatedValue, statValue) =>
                                    Math.Pow(statValue - averages[key], 2d) + accumulatedValue, key => averages[key]);
            Apply(stdDeviations, value => Math.Sqrt(value/statCount));
            sampleSet.AnalysisScratchPad.StdDeviations = stdDeviations;
        }

        public void ClusterAnalysis(ISampleSetAnalysis<ISampleAnalysis> sampleSet)
        {
            const int binMax = 5;
            AnalysisNote analysisNote;
            var sampleSetShortTypeName = sampleSet.GetType().ToString().Split('.').Last();
            string analysisNoteText = string.Format(@"
#Clustering Analysis ({0})

All the data is passed through the [openCV](http://opencv.org/) implementation of the [k-Means](http://en.wikipedia.org/wiki/K-means_clustering) clustering algorithm.

This is done several times to see if there are any 'obvious' groupings to the data.

The number of clusters is varied from 2, in powers of 2, up to {1} and plotted as a bar, where colour indicates cluster membership, and distance (left to right) represents time.",
            sampleSetShortTypeName, Math.Pow(2d, binMax));

            // Ensure statistics are associated correctly across all samples by ensuring we use a 
            // fixed order container of data value keys.
            // NOTE: tried to call the HashSet<String> asEnumerable, but got runtime binding errors
            // TODO: investigate runtime binding error for AllStatsHeaders.AsEnumerable().
            var keys = new List<string>();
            foreach (var val in sampleSet.AnalysisScratchPad.AllStatsHeaders)
            {
                keys.Add(val);
            }
            keys.Sort();

            var sampleCount = sampleSet.Count;
            var keysCount = keys.Count;
            var cvData = new CvMat(sampleCount, keysCount, MatrixType.F32C1);
            var cvClusters = new CvMat(sampleCount, 1, MatrixType.S32C1);

            // set up dynamic storage to save which cluster each sample belonged to,
            // for different numbers of cluster groups
            foreach (ISampleAnalysis sample in sampleSet)
            {
                if (null == sample)
                {
                    throw new ApplicationException("sample doesn't implement iSampleSetAnalysis");
                }
                sample.AnalysisScratchPad.Clusters = new Dictionary<int, int>();
            }

            // populate the OpenCV input data from the sample data
            for (var sampleIndex = 0; sampleIndex < sampleCount; sampleIndex++)
            {
                for (var keysIndex = 0; keysIndex < keysCount; keysIndex++)
                {
                    float data;
                    if (sampleSet[sampleIndex].ContainsKey(keys[keysIndex]))
                    {
                        data = (float) sampleSet[sampleIndex][keys[keysIndex]];
                    }
                    else
                    {
                        data = 0f;
                    }
                    cvData[sampleIndex, keysIndex] = data;
                }
            }

            // Going to repeatedly calculate clustering, with the number of clusters doubling each time.
            // But can't have more clusters than samples!
            var numberOfDoublings = (int) Math.Floor(Math.Log(sampleCount, 2));
            // must have at least one row; but on top of that there is a bug in oxyplot that causes
            // single row/column heatmaps to raise an overflow exception.
            if (numberOfDoublings < 2)
            {
                analysisNoteText = analysisNoteText + "\n\n*Not enough data to create chart*.\n\n";

                analysisNote = (AnalysisNote) Activator.CreateInstance(sampleSet.AnalysisNoteType,
                                                                           new object[]
                                                                               {
                                                                                   @"Clustering Analysis",
                                                                                   analysisNoteText,
                                                                                   AnalysisNoteDetailLevel.Summary,
                                                                                   AnalysisNotePriorities.Important,
                                                                                   new List<PlotModel>()
                                                                               });
                sampleSet.AddAnalysisNote(analysisNote);
                return;
            }

            if (numberOfDoublings > binMax)
            {
                numberOfDoublings = binMax;
            }

            // setup graph variables
            var clusterSizeAxis = new CategoryAxis {Title = "Cluster Size", Position =  AxisPosition.Left};
            var cvCriteria = new CvTermCriteria(10, 1.0);
            var heatMapData = new HeatMapSeries { Data = new Double[sampleCount, numberOfDoublings] };

            for (var power = 1; power <= numberOfDoublings; power++)
            {
                var clusterCount = (int) Math.Pow(2d, power);

                Cv.KMeans2(cvData, clusterCount, cvClusters, cvCriteria);
                var clusterName = string.Format("k:{0}", clusterCount);
                clusterSizeAxis.Labels.Add(clusterName);

                for (var sampleIndex = 0; sampleIndex < sampleCount; sampleIndex++)
                {
                    var cluster = cvClusters[sampleIndex];
                    var sample = sampleSet[sampleIndex];
                    sample.AnalysisScratchPad.Clusters[clusterCount] = (int)cluster;
                    heatMapData.Data[sampleIndex, power - 1] = cluster;
                }
            }

            var plot = new PlotModel {Title = "Clustering Analysis"};
            var colorAxis = new LinearColorAxis
                {
                    HighColor = OxyColors.Gray,
                    LowColor = OxyColors.Black,
                    Position = AxisPosition.Top
                };
            plot.Axes.Add(colorAxis);

            var startTime = sampleSet.StartTime;
            var endTime = sampleSet.EndTime;


            var timeAxis = new DateTimeAxis();
            timeAxis.Position = AxisPosition.Bottom;
            timeAxis.Minimum = DateTimeAxis.ToDouble(startTime);
            timeAxis.Maximum = DateTimeAxis.ToDouble(endTime);
            timeAxis.IntervalType = DateTimeIntervalType.Minutes;
            timeAxis.Title = "Time";
            timeAxis.StringFormat = "yy-MM-dd hh:mm:ss";
            timeAxis.Angle = -30;

            plot.Axes.Add(timeAxis);

            clusterSizeAxis.Minimum = -0.5;
            clusterSizeAxis.Maximum = numberOfDoublings - 0.5;
            clusterSizeAxis.Angle = -90;
            clusterSizeAxis.Position = AxisPosition.Left;
            plot.Axes.Add(clusterSizeAxis);

            heatMapData.X0 = timeAxis.Minimum;
            heatMapData.X1 = timeAxis.Maximum;
            heatMapData.Y0 = 0;
            heatMapData.Y1 = numberOfDoublings - 1;
            heatMapData.Interpolate = false;
            plot.Series.Add(heatMapData);


            analysisNote = (AnalysisNote) Activator.CreateInstance(sampleSet.AnalysisNoteType,
                                                                   new object[]
                                                                       {
                                                                           @"Clustering Analysis",
                                                                           analysisNoteText,
                                                                           AnalysisNoteDetailLevel.Summary,
                                                                           AnalysisNotePriorities.Important,
                                                                           new List<PlotModel> {plot}
                                                                       });
            sampleSet.AddAnalysisNote(analysisNote);
        }


        public static AnalysisNote CreateGraphs(string title, string summary, ISampleSetAnalysis<ISampleAnalysis> sampleSet, List<Dictionary<object, object>> statsToGraph )
        {

            var averages = sampleSet.AnalysisScratchPad.Averages;
            var stdDeviations = sampleSet.AnalysisScratchPad.StdDeviations;

            var graphs = new List<PlotModel>();
            var notes = new StringBuilder();

            foreach (var graphSpec in statsToGraph)
            {
                var graphStats = graphSpec["Statistics"];
                var statSeriesObjects = graphStats as List<object> ?? new List<object> {graphStats};

                if (0 == statSeriesObjects.Count)
                {
                    throw new ArgumentException("No statistic specified for graph!");
                }

                var regexStatSeries = statSeriesObjects.Select(s => s.ToString()).ToList();
                
                var statSeries = new List<string>();
                foreach (string statHeader in sampleSet.AnalysisScratchPad.AllStatsHeaders)
                {
                    statSeries.AddRange(from statRegex in regexStatSeries where Regex.IsMatch(statHeader, statRegex) select statHeader);
                }

                var includeAverage = (graphSpec["IncludeAverage"].ToString() == "true");
                var includeStdDev = (graphSpec["IncludeStdDeviation"].ToString() == "true");

                if (includeStdDev && !includeAverage)
                {
                    throw new ArgumentException("Can't include add std deviation to graph with out the average.");
                }

                var graph = new PlotModel { Title = graphSpec["Title"].ToString() };
                var xAxis = new DateTimeAxis
                {
                    Position = AxisPosition.Bottom,
                    Minimum = DateTimeAxis.ToDouble(sampleSet.StartTime),
                    Maximum = DateTimeAxis.ToDouble(sampleSet.EndTime)
                };
                graph.Axes.Add(xAxis);

                var yAxis = new LinearAxis { Position = AxisPosition.Left };
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

                            stdDevAreaSeries.Points.Add(new DataPoint(timeStamp, average + stdDev));
                            stdDevAreaSeries.Points2.Add(new DataPoint(timeStamp, average - stdDev));
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

            var analysisSummary = new StringBuilder(summary);

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
            var analysisNote = (AnalysisNote) Activator.CreateInstance(sampleSet.AnalysisNoteType,
                                                                       new object[]
                                                                           {
                                                                               title,
                                                                               analysisSummary.ToString(),
                                                                               AnalysisNoteDetailLevel.Summary,
                                                                               AnalysisNotePriorities.Important,
                                                                               graphs
                                                                           });
            return analysisNote;
        }
    }
}
