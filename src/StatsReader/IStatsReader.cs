using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace StatsReader
{
    public interface IStatsReader
    {
        // Given a TextReader stream, parse it and store it internally
        void ParseInput(TextReader input);

    }
}
