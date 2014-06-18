using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using NorwegianBlue.Data.SampleSet;
using NorwegianBlue.Integration.Iago.Analysis;
using NorwegianBlue.Integration.Iago.Data.Sample;

namespace NorwegianBlue.Integration.Iago.Data.SampleSet
{
    public class IagoSampleSet : BaseSampleSetWithAnalysis<IagoSample>
    {
        // Example line:
        // INF [20140129-16:09:01.218] stats: {...}
        private static readonly Regex LineMatcher = new Regex(@"^INF \[(?<year>\d{4})(?<month>\d{2})(?<day>\d{2})-(?<hour>\d{2}):(?<minute>\d{2}):(?<second>\d{2})\.(?<millis>\d{3})\] stats: \{(?<stats>.*)\}$", RegexOptions.Compiled);
        private static readonly Regex DataMatcher = new Regex(@"(?<key>.+?):(?<value>.+?)[,$]", RegexOptions.Compiled);
        private static readonly Regex QuotteRemover = new Regex("([\"'])(?<key>.+?)\\1", RegexOptions.Compiled);
        private static readonly Regex VSlashRemover = new Regex("\\\\/", RegexOptions.Compiled);

        public override Type AnalysisNoteType
        {
            get { return typeof(IagoSampleAnalysisNote); }
        }

        internal IagoSampleSet(IReadOnlyDictionary<object, object> configuration ): base(configuration)
        {
        }

        protected override IEnumerable<IagoSample> DoParse(TimeZone desiredTimeZone, DateTime? startTime, DateTime? endTime, dynamic dataSourceConfigObject)
        {
            var iagoSamples = new List<IagoSample>();

            TextReader inputReaderStream;
            
            if (
                ((IDictionary<string, Object>)dataSourceConfigObject).ContainsKey("DataSourceStream"))
            {
                inputReaderStream = dataSourceConfigObject.DataSourceStream;
            }
            else
            {
                string fileName = dataSourceConfigObject.DataSourceFileName;
                inputReaderStream = new StreamReader(fileName);
            }

            using (inputReaderStream)
            {
                var iagoSampleFactory = new IagoSampleFactory();

                string input;
                while ((input = inputReaderStream.ReadLine()) != null)
                {
                    var matches = LineMatcher.Match(input);

                    if (!matches.Success)
                    {
                        throw new InvalidDataException("input line can't be parsed by Iago parser: " + input);
                    }


                    var timeStamp = new DateTime(Convert.ToInt32(matches.Groups["year"].Value),
                                                Convert.ToInt32(matches.Groups["month"].Value),
                                                Convert.ToInt32(matches.Groups["day"].Value),
                                                Convert.ToInt32(matches.Groups["hour"].Value),
                                                Convert.ToInt32(matches.Groups["minute"].Value),
                                                Convert.ToInt32(matches.Groups["second"].Value),
                                                Convert.ToInt32(matches.Groups["millis"].Value)
                                            );
                    var data = matches.Groups["stats"].Value;

                    IList<Tuple<string, string>> sampleData = new List<Tuple<string, string>>();

                    foreach (Match kvpmatch in DataMatcher.Matches(data))
                    {
                        var key = kvpmatch.Groups["key"].Value;
                        var temp = QuotteRemover.Match(key);
                        key = temp.Groups["key"].Value;
                        key = VSlashRemover.Replace(key, "/");
                        var value = kvpmatch.Groups["value"].Value;
                        sampleData.Add(new Tuple<string, string>(key, value));
                    }
                    var iagoSample = iagoSampleFactory.Create(timeStamp, sampleData);
                    iagoSamples.Add(iagoSample);
                }
            }

            return iagoSamples;
        }
    }
}