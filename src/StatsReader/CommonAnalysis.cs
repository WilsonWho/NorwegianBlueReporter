using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using OpenCvSharp;

namespace StatsReader
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

            var keyStats = new List<string> { @"client\/request_latency_ms", "" };




            //            var avgData = averages.Select(kvp => new Tuple<dynamic, dynamic>(kvp.Key, kvp.Value)).ToList();
            //            var avgSeries = new List<SeriesData>() { new SeriesData("Variable", avgData) };
            //            var avgGraph = new Graph("Averages", GraphType.Bar, avgSeries);
            //
            //            var analysisNote = new AnalysisNote("Interval Averages",
            //@"#Title- Interval Averages
            //
            //This is some paragraph text with **bold**
            //
            //",
            //                                                avgGraph);
            //
            //            statSet.AddAnalysisNote(analysisNote);



        }

        public void ClusterAnalysis(IStatisticsSetAnalysis statSet)
        {
            int statCount = statSet.Statistics.Count;

            var keys = new List<string>();
            foreach (var val in statSet.AnalysisScratchPad.AllStatsHeaders)
            {
                keys.Add(val);
            }
            keys.Sort();

            int rowIdx = 0;
            var data = new CvMat(statCount, keys.Count, MatrixType.F32C1);
            var clusters = Cv.CreateMat(statCount, 1, MatrixType.S32C1);

            foreach (var row in statSet.Statistics)
            {
                // putting data initialization for next step here to avoid another loop over all the statistics collected
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

            for (int power = 1; power < 3; power++)
            {
                int clusterCount = (int) Math.Pow(2d, (float) power);
                var x = OpenCvSharp.Cv.KMeans2(data, clusterCount, clusters, new CvTermCriteria(10, 1.0));

                rowIdx = 0;
                foreach (IStatisticsAnalysis stat in statSet.Statistics)
                {
                    stat.AnalysisScratchPad.Clusters[clusterCount] = (int) clusters[rowIdx];
                    rowIdx++;
                }
            }
        }
    }

    public class CommonStatAnalysis
    {
        public void SummaryStatComparison(IStatisticsSetAnalysis statSet, IStatisticsAnalysis stat)
        {
            var stdDevs = new StringBuilder();
            var missingFields = new StringBuilder();

            foreach (string item in statSet.AnalysisScratchPad.AllStatsHeaders)
            {
                if (stat.Stats.ContainsKey(item))
                {
                    var numberOfSigmaFromMean = Math.Abs(stat.Stats[item] - statSet.AnalysisScratchPad.Averages[item]) / statSet.AnalysisScratchPad.StdDeviations[item];
                    if (numberOfSigmaFromMean > 1)
                    {
                        if (stdDevs.Length > 0)
                        {
                            stdDevs.Append(", ");
                        }
                        stdDevs.AppendFormat("{0} [{1}]", item, Math.Round(numberOfSigmaFromMean, 2));
                    }
                }
                else
                {
                    if (missingFields.Length > 0)
                    {
                        missingFields.Append(", ");
                    }
                    missingFields.AppendFormat("{0}", item);
                }
            }
            
            if (stdDevs.Length > 0)
            {
                stdDevs.Insert(0, "*Fields more than 1 std dev from mean*: ");
                stdDevs.Append(".");
                var analysisNote = new AnalysisNote("Stat Summary", stdDevs.ToString(), null);
                stat.AddAnalysisNote(analysisNote);
            }

            if (missingFields.Length > 0)
            {
                missingFields.Insert(0, "*Missing fields*: ");
                stdDevs.Append(".");
                var analysisNote = new AnalysisNote("Missing Fields", missingFields.ToString(), null);
                stat.AddAnalysisNote(analysisNote);
            }

        }
    }
}

