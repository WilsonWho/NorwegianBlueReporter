using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using OpenCvSharp;

namespace LaserOptics.Common
{
    public class CommonStatSetAnalysis
    {
        public void FindAllHeaders(IStatisticsSetAnalysis statSet)
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

        private static void LoopOverStatsAndHeaders(IStatisticsSetAnalysis statSet, Dictionary<string, double> values, Func<string, double, double, double> action, Func<string, double> defaultValue)
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

        public void SummaryStats(IStatisticsSetAnalysis statSet)
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

        public void ClusterAnalysis(IStatisticsSetAnalysis statSet)
        {
            var keys = new List<string>();

            foreach (var val in statSet.AnalysisScratchPad.AllStatsHeaders)
            {
                keys.Add(val);
            }

            keys.Sort();

            var labels = new List<string>();
            int statCount = statSet.Count;
            int rowIdx = 0;
            var data = new CvMat(statCount, keys.Count, MatrixType.F32C1);
            var clusters = Cv.CreateMat(statCount, 1, MatrixType.S32C1);

            foreach (var row in statSet)
            {
                // putting data initialization for next step here to avoid another loop over all the statistics collected
                labels.Add(row.TimeStamp.ToString(CultureInfo.InvariantCulture));

                var row2 = row as IStatisticsAnalysis;
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


            var series = new List<SeriesData>();
            for (int power = 1; power < 5; power++)
            {
                var clusterCount = (int) Math.Pow(2d, power);
                if (clusterCount > statCount)
                {
                    break;
                }

                Cv.KMeans2(data, clusterCount, clusters, new CvTermCriteria(10, 1.0));

                rowIdx = 0;
                var clusterSeriesName = string.Format("k:{0}", clusterCount);
                var clusterSeriesData = new List<double>();
                foreach (IStatisticsAnalysis stat in statSet)
                {
                    var cluster = clusters[rowIdx];
                    stat.AnalysisScratchPad.Clusters[clusterCount] = (int) cluster;
                    clusterSeriesData.Add(cluster);
                    rowIdx++;
                }
                series.Add(new SeriesData(clusterSeriesName, clusterSeriesData));
            }

            var graph = new GraphData("Clustering",
                                      labels,
                                      false,
                                      LegendPositionEnum.Left,
                                      false,
                                      GraphType.ColorTable,
                                      series);

            var analysisNote = new AnalysisNote("Clustering Analysis",
@"
#Clustering Analysis

All the data is passed through the [openCV](http://opencv.org/) implementation of the [k-Means](http://en.wikipedia.org/wiki/K-means_clustering) clustering algorithm.

This is done several times to see if there are any 'obvious' groupings to the data.

The number of clusters is varied from 2, in powers of 2, up to 16 and plotted as a bar, where colour indicates cluster membership, and distance (left to right) represents time.

",
                                                graph);
            statSet.AddAnalysisNote(analysisNote);
        }
    }
}

