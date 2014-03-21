using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NorwegianBlue.Analysis.Samples;
using OpenCvSharp;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace NorwegianBlue.Analysis.CommonAlgorithms
{
    public class CommonStatSetAnalysis
    {
        public void FindAllHeaders(ISampleSetAnalysis statSet)
        {
            var statsHeaders = new HashSet<String>();
            var nonStatsHeaders = new HashSet<String>();

            foreach (var stat in statSet)
            {
                statsHeaders.UnionWith(stat.Keys);
                nonStatsHeaders.UnionWith(stat.NonStats.Keys);
            }

            statSet.AnalysisScratchPad.AllStatsHeaders = statsHeaders;
            statSet.AnalysisScratchPad.AllNonStatsHeaders = nonStatsHeaders;
        }

        private static void LoopOverStatsAndHeaders(ISampleSetAnalysis statSet, Dictionary<string, double> values, Func<string, double, double, double> action, Func<string, double> defaultValue)
        {
            foreach (var stat in statSet)
            {
                foreach (string key in statSet.AnalysisScratchPad.AllStatsHeaders)
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

        public void SummaryStats(ISampleSetAnalysis statSet)
        {
            int statCount = statSet.Count;

            var averages = new Dictionary<string, double>();
            LoopOverStatsAndHeaders(statSet, averages,
                                    (key, accumulatedValue, statValue) => accumulatedValue + statValue,
                                    arg => 0d);

            Apply(averages, value => value/statCount);
            statSet.AnalysisScratchPad.Averages = averages;

            var stdDeviations = new Dictionary<string, double>();
            // if there is a missing value use the average value for the field as the 'default' value, rather than zero (ie try not to inflate the std deviation.)
            LoopOverStatsAndHeaders(statSet, stdDeviations,
                                    (key, accumulatedValue, statValue) =>
                                    Math.Pow(statValue - averages[key], 2d) + accumulatedValue, key => averages[key]);
            Apply(stdDeviations, value => Math.Sqrt(value/statCount));
            statSet.AnalysisScratchPad.StdDeviations = stdDeviations;
        }

        public void ClusterAnalysis(ISampleSetAnalysis statSet)
        {
            var keys = new List<string>();

            foreach (var val in statSet.AnalysisScratchPad.AllStatsHeaders)
            {
                keys.Add(val);
            }

            keys.Sort();

            int statCount = statSet.Count;
            var data = new CvMat(statCount, keys.Count, MatrixType.F32C1);
            var clusters = Cv.CreateMat(statCount, 1, MatrixType.S32C1);

            // populate the OpenCV input data from the sample data
            int rowIdx = 0;
            foreach (var row in statSet)
            {
                // putting data initialization for next step here to avoid another loop over all the statistics collected
                var row2 = row as ISampleAnalysis;
                Debug.Assert(row2 != null, "row2 != null");
                row2.AnalysisScratchPad.Clusters = new Dictionary<int, int>();

                int colIdx = 0;
                foreach (var k in keys)
                {
                    float temp;
                    if (row.ContainsKey(k))
                    {
                        temp = (float) row[k];
                    }
                    else
                    {
                        temp = 0f;
                    }
                    data[rowIdx, colIdx] = temp;
                    colIdx ++;
                }
                rowIdx++;
            }

            var clusterSizeAxis = new CategoryAxis {Title = "Cluster Size", Position =  AxisPosition.Left};
            var cvCriteria = new CvTermCriteria(10, 1.0);

            var series = new List<List<double>>();
            for (int power = 1; power < 5; power++)
            {
                var clusterCount = (int) Math.Pow(2d, power);
                if (clusterCount > statCount)
                {
                    break;
                }

                Cv.KMeans2(data, clusterCount, clusters, cvCriteria);
                var clusterName = string.Format("k:{0}", clusterCount);
                clusterSizeAxis.Labels.Add(clusterName);
                var clusterSeriesData = new List<double>();
                rowIdx = 0;
                foreach (ISampleAnalysis stat in statSet)
                {
                    var cluster = clusters[rowIdx];
                    stat.AnalysisScratchPad.Clusters[clusterCount] = (int)cluster;
                    // convert from the OpenCV matrix to a regular C# List so we can use
                    // this with OxyPlot
                    clusterSeriesData.Add(cluster);
                    rowIdx++;
                }
                series.Add(clusterSeriesData);
            }

            var plotModel = new PlotModel();
            plotModel.Title = "Clustering Analysis";
            var linearColorAxis = new LinearColorAxis
                {
                    HighColor = OxyColors.Gray,
                    LowColor = OxyColors.Black,
                    Position = AxisPosition.Right
                };
            plotModel.Axes.Add(linearColorAxis);

            var timeAxis = new DateTimeAxis(AxisPosition.Bottom, "Category Analysis", "yy-MM-dd hh:mm:ss");
            plotModel.Axes.Add(timeAxis);
            plotModel.Axes.Add(clusterSizeAxis);

            var heatMapSeries = new HeatMapSeries {Data = new Double[statSet.Count,series.Count]};

            for (int y = 0; y < series.Count; y++)
            {
                for (int x = 0; x < statSet.Count; x++)
                {
                    heatMapSeries.Data[x, y] = series[y][x];
                }
                
            }

            plotModel.Series.Add(heatMapSeries);

            var analysisNote = new AnalysisNote("Clustering Analysis",
@"
#Clustering Analysis

All the data is passed through the [openCV](http://opencv.org/) implementation of the [k-Means](http://en.wikipedia.org/wiki/K-means_clustering) clustering algorithm.

This is done several times to see if there are any 'obvious' groupings to the data.

The number of clusters is varied from 2, in powers of 2, up to 16 and plotted as a bar, where colour indicates cluster membership, and distance (left to right) represents time.

", AnalysisNote.AnalysisNoteType.Summary, AnalysisNote.AnalysisNotePriorities.Important, new List<PlotModel>{plotModel});
            statSet.AddAnalysisNote(analysisNote);
        }
    }
}
