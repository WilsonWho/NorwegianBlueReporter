using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsReader
{
    class IagoStatisticsSet: IStatisticsSet
    {
        private List<IagoStatistics> _iagoStatistics = new List<IagoStatistics>(); 

        public ReadOnlyCollection<IStatisticsValues> Statistics { get { return Utils.ReadOnlyCollectionWithCopiedValues(_iagoStatistics.Cast<IStatisticsValues>()); } 
        }
        public void Parse(TextReader input)
        {
            throw new NotImplementedException();
        }

        public void Analyze()
        {
            throw new NotImplementedException();
        }
    }
}
