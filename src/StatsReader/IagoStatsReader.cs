using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StatsReader
{
    class IagoStatsReader : IStatsReader
    {
        // Example line:
        // INF [20140129-16:09:01.218] stats: {...}
        private static readonly Regex lineMatcher = new Regex(@"^INF \[(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})-(?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2})\.(?<millis>\d{3})\] stats: \{(?<stats>.*)\}$", RegexOptions.Compiled);
        private static readonly Regex dataMatcher = new Regex(@"(?<key>.+?):(?<value>.+?)[,$]", RegexOptions.Compiled);

        // all the processed stats lines
        private List<IagoStatistics> statsLines;
        // all the headers encountered- not every stats line will have every header
        private Dictionary<String, int> headers; 

        public void ParseInput(TextReader input)
        {
            string line;
            while ((line = input.ReadLine()) != null)
            {
                var matches = lineMatcher.Match(line);

                if (!matches.Success)
                {
                    throw new InvalidDataException("input line can't be parsed by Iago parser: " + line);
                }

                var statsLine = new IagoStatistics();

                statsLine.TimeStamp = new DateTime( Convert.ToInt32(matches.Groups["year"].Value),
                                                   Convert.ToInt32(matches.Groups["month"].Value),
                                                   Convert.ToInt32(matches.Groups["day"].Value),
                                                   Convert.ToInt32(matches.Groups["hour"].Value),
                                                   Convert.ToInt32(matches.Groups["minute"].Value),
                                                   Convert.ToInt32(matches.Groups["second"].Value),
                                                   Convert.ToInt32(matches.Groups["millis"].Value)
                                                  );
                string data = matches.Groups["stats"].Value;


                foreach (Match kvpmatch in dataMatcher.Matches(data))
                {
                    String key = kvpmatch.Groups["key"].Value;
                    String value = kvpmatch.Groups["value"].Value;

                    // keep a count of the number of times each header is encountered.
                    // Can be used to easily determine which fields, if any, aren't present in all the data
                    if (headers.ContainsKey(key))
                    {
                        headers[key]++;
                    }
                    else
                    {
                        headers[key] = 1;
                    }

                    if (!statsLine.Stats.ContainsKey(key))
                    {
                        statsLine.Stats[key] = value;
                    }
                    else
                    {
                        throw new InvalidDataException("input line has duplicate data intems and can't be parsed by Iago parser. Data name: " + key + ", line: " + line);
                    }
                }

                Console.WriteLine("Match output - {0}", eventTime.ToString());
                Console.WriteLine("-----------");
            }
        }
    }
}
