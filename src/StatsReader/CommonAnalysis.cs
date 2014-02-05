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


    class CommonStatSetAnalysis
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

        public void SummaryStats(IStatisticsSetAnalysis statSet)
        {
            var averages = new Dictionary<string, double>();

            foreach (var stat in statSet.Statistics)
            {
                foreach (string key in statSet.AnalysisScratchPad.AllStatsHeaders)
                {
                    if (!averages.ContainsKey(key))
                    {
                        averages[key] = 0d;
                    } 

                    if (stat.Stats.ContainsKey(key))
                    {
                        averages[key] += stat.Stats[key];
                    }
                }
            }

            int statCount = statSet.Statistics.Count;

            foreach (var key in averages.Keys.ToList())
            {
                averages[key] = averages[key]/statCount;
            }

            statSet.AnalysisScratchPad.Averages = averages;
        }
    }
}
