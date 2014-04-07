using System;
using System.IO;
using System.Text.RegularExpressions;
using NorwegianBlue.Samples;

namespace NorwegianBlue.IagoIntegration.Samples
{
    public class IagoSample : CommonSampleBase 
    {
        // Example line:
        // INF [20140129-16:09:01.218] stats: {...}
        private static readonly Regex LineMatcher = new Regex(@"^INF \[(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})-(?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2})\.(?<millis>\d{3})\] stats: \{(?<stats>.*)\}$", RegexOptions.Compiled);
        private static readonly Regex DataMatcher = new Regex(@"(?<key>.+?):(?<value>.+?)[,$]", RegexOptions.Compiled);
        private static readonly Regex QuotteRemover = new Regex("([\"'])(?<key>.+?)\\1", RegexOptions.Compiled);
        private static readonly Regex VSlashRemover = new Regex("\\\\/", RegexOptions.Compiled);

        public IagoSample():base()
        {
//            Dictionary<object, object> configuration = YamlParser.GetConfiguration();
//            AnalysisScratchPad.FieldsToIgnore = configuration.ContainsKey("FieldsToIgnore")
//                                                    ? configuration["FieldsToIgnore"]
//                                                    : new List<string>();
        }

        public void Parse(TimeZone timeZone, String input)
        {
            var matches = LineMatcher.Match(input);

            if (!matches.Success)
            {
                throw new InvalidDataException("input line can't be parsed by Iago parser: " + input);
            }


            TimeStamp = new DateTime(   Convert.ToInt32(matches.Groups["year"].Value),
                                        Convert.ToInt32(matches.Groups["month"].Value),
                                        Convert.ToInt32(matches.Groups["day"].Value),
                                        Convert.ToInt32(matches.Groups["hour"].Value),
                                        Convert.ToInt32(matches.Groups["minute"].Value),
                                        Convert.ToInt32(matches.Groups["second"].Value),
                                        Convert.ToInt32(matches.Groups["millis"].Value)
                                    );
            string data = matches.Groups["stats"].Value;

            foreach (Match kvpmatch in DataMatcher.Matches(data))
            {
                var key = kvpmatch.Groups["key"].Value;
                var temp = QuotteRemover.Match(key);
                key = temp.Groups["key"].Value;
                key = VSlashRemover.Replace(key, "/");

                String value = kvpmatch.Groups["value"].Value;

                if (Stats.ContainsKey(key) || UpdateableNonStats.ContainsKey(key))
                {
                    throw new InvalidDataException(
                        "input line has duplicate data intems and can't be parsed by Iago parser. Data name: " + key +
                        ", line: " + input);
                }

                AddParsedData(key, value);
            }
        }
    }
}
