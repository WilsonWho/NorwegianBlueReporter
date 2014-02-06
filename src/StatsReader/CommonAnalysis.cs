using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        private static void Normalize(Dictionary<string, double> values, Func<double, double> normalizer )
        {
            foreach (var key in values.Keys.ToList())
            {
                values[key] = normalizer(values[key]);
            }
        }

        public void SummaryStats(IStatisticsSetAnalysis statSet)
        {
            int statCount = statSet.Statistics.Count;

            var averages = new Dictionary<string, double>();
            LoopOverStatsAndHeaders(statSet, averages, (key, accumulatedValue, statValue) => accumulatedValue + statValue, (key) => 0d);
            Normalize(averages, (value) => value / statCount);
            statSet.AnalysisScratchPad.Averages = averages;

            var avgData = averages.Select(kvp => new Tuple<dynamic, dynamic>(kvp.Key, kvp.Value)).ToList();
            var avgSeries = new List<SeriesData>() {new SeriesData("Variable", avgData)};
            var avgGraph = new Graph("Averages", GraphType.Bar, avgSeries);

            var analysisNote = new AnalysisNote("Interval Averages",
@"#Title- Interval Averages

This is some paragraph text with **bold**

",
                                                avgGraph);

            statSet.AddAnalysisNote(analysisNote);

            var stdDeviations = new Dictionary<string, double>();
            // if there is a missing value use the average value for the field as the 'default' value, rather than zero (ie try not to inflate the std deviation.)
            LoopOverStatsAndHeaders(statSet, stdDeviations, (key, accumulatedValue, statValue) => Math.Pow(statValue - averages[key], 2d) + accumulatedValue, (key) => averages[key]);
            Normalize(stdDeviations, (value) => Math.Sqrt(value/statCount));
            statSet.AnalysisScratchPad.StdDeviations = stdDeviations;


        }

    }
}
