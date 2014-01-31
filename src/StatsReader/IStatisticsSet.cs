using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsReader
{
    interface IStatisticsSet
    {
        ReadOnlyCollection<IStatisticsValues> Statistics { get; }

        void Parse(TextReader input);
        void Analyze();

    }

}
