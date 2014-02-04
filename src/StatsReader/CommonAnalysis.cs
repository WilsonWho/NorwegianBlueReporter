using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsReader
{
    class CommonAnalysis
    {
        public HashSet<String> FindAllHeaders(IEnumerable<IStatisticsValues> statistics)
        {
            var results = new HashSet<String>();

            foreach (var stat in statistics)
            {
                results.UnionWith(stat.Stats.Keys);
            }

            return results;
        } 
    }
}
