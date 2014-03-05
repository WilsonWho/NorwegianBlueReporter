using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using OpenCvSharp;

namespace LaserOptics.Common
{
    // thought: StatsSets have access to the individual stats, but stats only have access to themselves.
    // Thus analyzing individual stats happens AFTER analyzing the set, and may require information from
    // the analysis of the set.
    // Also: usign an expando object in the stat sets and stats to store analysis results and 
    // temporary values- to allow free form, low-overhead, dependencies between different statistical
    // analysis.  If repeating patterns reveal themselves this might be refactored into "proper" interfaces.
    public delegate void SetAnalyzer(IStatisticsSetAnalysis statSet);
    public delegate void StatAnalyzer(IStatisticsSetAnalysis statSet, IStatisticsAnalysis stat);


//            private readonly float _iagoStatAllowedRequestResponseDifference = float.Parse(ConfigurationManager.AppSettings["IagoStatAllowedRequestResponseDifference"], CultureInfo.InvariantCulture.NumberFormat);


    public class CommonStatSetAnalysis
    {
        public void FindAllHeaders(IStatisticsSetAnalysis statSet)
        {
            var statsHeaders = new HashSet<String>();
            var nonStatsHeaders = new HashSet<String>();

            foreach (var stat in statSet.Statistics)
            {
                statsHeaders.UnionWith(stat.Stats.Keys);
                nonStatsHeaders.UnionWith(stat.NonStats.Keys);
            }

            statSet.AnalysisScratchPad.AllStatsHeaders = statsHeaders;
            statSet.AnalysisScratchPad.AllNonStatsHeaders = nonStatsHeaders;
        }

        private static void LoopOverStatsAndHeaders(IStatisticsSetAnalysis statSet, Dictionary<string, double> values, Func<string, double, double, double> action, Func<string, double> defaultValue)
        {
            foreach (var stat in statSet.Statistics)
            {
                foreach (string key in statSet.AnalysisScratchPad.AllStatsHeaders)
                {
                    if (!values.ContainsKey(key))
                    {
                        values[key] = defaultValue(key);
                    }
                    if (stat.Stats.ContainsKey(key))
                    {
                        values[key] = action(key, values[key], stat.Stats[key]);
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
            int statCount = statSet.Statistics.Count;

            var averages = new Dictionary<string, double>();
            LoopOverStatsAndHeaders(statSet, averages,
                                    (key, accumulatedValue, statValue) => accumulatedValue + statValue, (key) => 0d);

            Apply(averages, (value) => value/statCount);
            statSet.AnalysisScratchPad.Averages = averages;

            var stdDeviations = new Dictionary<string, double>();
            // if there is a missing value use the average value for the field as the 'default' value, rather than zero (ie try not to inflate the std deviation.)
            LoopOverStatsAndHeaders(statSet, stdDeviations,
                                    (key, accumulatedValue, statValue) =>
                                    Math.Pow(statValue - averages[key], 2d) + accumulatedValue, (key) => averages[key]);
            Apply(stdDeviations, (value) => Math.Sqrt(value/statCount));
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
            int statCount = statSet.Statistics.Count;
            int rowIdx = 0;
            var data = new CvMat(statCount, keys.Count, MatrixType.F32C1);
            var clusters = Cv.CreateMat(statCount, 1, MatrixType.S32C1);

            foreach (var row in statSet.Statistics)
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
                    if (row.Stats.ContainsKey(k))
                    {
                        temp = (float) row.Stats[k];
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
                int clusterCount = (int) Math.Pow(2d, (float) power);
                if (clusterCount > statCount)
                {
                    break;
                }

                var x = Cv.KMeans2(data, clusterCount, clusters, new CvTermCriteria(10, 1.0));

                rowIdx = 0;
                var clusterSeriesName = string.Format("k:{0}", clusterCount);
                var clusterSeriesData = new List<double>();
                foreach (IStatisticsAnalysis stat in statSet.Statistics)
                {
                    var cluster = clusters[rowIdx];
                    stat.AnalysisScratchPad.Clusters[clusterCount] = (int) cluster;
                    clusterSeriesData.Add((double) cluster);
                    rowIdx++;
                }
                series.Add(new SeriesData(clusterSeriesName, clusterSeriesData));
            }

            var graph = new GraphData("Clustering",
                                      labels,
                                      false,
                                      LegendPositionEnum.Left,
                                      false,
                                      GraphType.ColorTableGraph,
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

    public class CommonStatAnalysis
    {
        // MigraDoc/PDFSharp don't wrap cell contents... so this needs quite a lot of work to be a reasonable
        // presentation
        // TODO: make this usable
        public void SummaryStatComparisonAsTables(IStatisticsSetAnalysis statSet, IStatisticsAnalysis stat)
        {
            var stdDevs = new StringBuilder();
            var missingFields = new StringBuilder();

            const int numStdDevCols = 1;
            const int numMissingValCols = 1;

            var stdDevHeader = new StringBuilder("*Fields more than 1 std dev from mean*:\n\n");
            var stdDevTableHeader1 = new StringBuilder("Field|Standard-Deviations-from-Mean");
            var stdDevTableHeader2 = new StringBuilder("------------------|-----------------");
            for (int i = 0; i < numStdDevCols - 1; i++)
            {
                stdDevTableHeader1.Append("|Field|Standard-Deviations-from-Mean");
                stdDevTableHeader2.Append("|------------------|-----------------");
            }
            stdDevTableHeader1.Append("\n");
            stdDevTableHeader2.Append("\n");
            stdDevHeader.Append(stdDevTableHeader1);
            stdDevHeader.Append(stdDevTableHeader2);

            var missingValHeader = new StringBuilder("*Missing fields:*\n\n");
            var missingValTableHeader1 = new StringBuilder("Field");
            var missingValTableHeader2 = new StringBuilder("------------------");
            for (int i = 0; i < numMissingValCols - 1; i++)
            {
                missingValTableHeader1.Append("|Field");
                missingValTableHeader2.Append("|------------------");
            }
            missingValTableHeader1.Append("|\n");
            missingValTableHeader2.Append("|\n");
            missingValHeader.Append(missingValTableHeader1);
            missingValHeader.Append(missingValTableHeader2);

            int stdDevCol = 0;
            int missingValCol = 0;
            foreach (string item in statSet.AnalysisScratchPad.AllStatsHeaders)
            {
                var hyphenatedItem = item.InsertSoftHyphens();

                if (stat.Stats.ContainsKey(item))
                {
                    var numberOfSigmaFromMean = Math.Abs(stat.Stats[item] - statSet.AnalysisScratchPad.Averages[item]) / statSet.AnalysisScratchPad.StdDeviations[item];
                    if (numberOfSigmaFromMean > 1)
                    {
                        stdDevCol++;
                        stdDevs.AppendFormat("``{0}``|{1}", hyphenatedItem, Math.Round(numberOfSigmaFromMean, 2));
                        if (stdDevCol< numStdDevCols)
                        {
                            stdDevs.Append("|");
                        } 
                        else
                        {
                            stdDevCol = 0;
                            stdDevs.Append("\n");
                        }
                    }
                }
                else
                {
                    missingValCol++;
                    missingFields.AppendFormat("``{0}``", hyphenatedItem);
                    if (missingValCol < numMissingValCols)
                    {
                        missingFields.Append("|");
                    }
                    else
                    {
                        missingValCol = 0;
                        missingFields.Append("|\n");
                    }
                }
            }
            
            if (stdDevs.Length > 0)
            {
                if (stdDevCol != 0)
                {
                    for (int i = stdDevCol; i < numStdDevCols; i++)
                    {
                        stdDevs.Append("||");
                    }
                    stdDevs.Append("|\n");
                }
                stdDevs.Insert(0, stdDevHeader);
                stdDevs.Append("\n\n");
                var analysisNote = new AnalysisNote("Stat Summary", stdDevs.ToString());
                stat.AddAnalysisNote(analysisNote);
            }

            if (missingFields.Length > 0)
            {
                if (missingValCol != 0)
                {
                    for (int i = missingValCol; i < numMissingValCols; i++)
                    {
                        missingFields.Append("|");
                    }
                    missingFields.Append("\n");
                }
                missingFields.Insert(0, missingValHeader);
                missingFields.Append("\n\n");
                var analysisNote = new AnalysisNote("Missing Fields", missingFields.ToString());
                stat.AddAnalysisNote(analysisNote);
            }

        }

        public void SummaryStatComparison(IStatisticsSetAnalysis statSet, IStatisticsAnalysis stat)
        {
            var stdDevs = new StringBuilder();
            var missingFields = new StringBuilder();

            foreach (string item in statSet.AnalysisScratchPad.AllStatsHeaders)
            {
                var hyphenatedItem = item.InsertSoftHyphens();

                if (stat.Stats.ContainsKey(item))
                {
                    var numberOfSigmaFromMean = Math.Abs(stat.Stats[item] - statSet.AnalysisScratchPad.Averages[item]) / statSet.AnalysisScratchPad.StdDeviations[item];
                    if (numberOfSigmaFromMean > 1)
                    {
                        stdDevs.AppendFormat("``{0}`` \\[{1}\\]\n", hyphenatedItem, Math.Round(numberOfSigmaFromMean, 2));
                    }
                }
                else
                {
                    missingFields.AppendFormat("``{0}``\n", hyphenatedItem);
                }
            }

            if (stdDevs.Length > 0)
            {
                stdDevs.Insert(0, "####Fields more than 1 std dev from mean:\n");
                stdDevs.Append("\n\n");
                var analysisNote = new AnalysisNote("Stat Summary", stdDevs.ToString());
                stat.AddAnalysisNote(analysisNote);
            }

            if (missingFields.Length > 0)
            {
                missingFields.Insert(0, "####Missing fields:\n");
                missingFields.Append("\n\n");
                var analysisNote = new AnalysisNote("Missing Fields", missingFields.ToString());
                stat.AddAnalysisNote(analysisNote);
            }

        }
    }
}

