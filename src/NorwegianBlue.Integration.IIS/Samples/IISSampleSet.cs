using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NorwegianBlue.Analysis;
using NorwegianBlue.Analysis.Samples;
using NorwegianBlue.Util.Configuration;
using NorwegianBlue.Util.Web;

namespace NorwegianBlue.Integration.IIS.Samples
{
    public class IISSampleSet : ISampleSet, ISampleSetAnalysis
    {
        private Dictionary<string, object> _configuration;

        public IISSampleSet()
        {
            _configuration = YamlParser.GetConfiguration();
        }

        public void Parse(TimeZone timeZone, string dataLocation, DateTime? startTime, DateTime? endTime)
        {
            var uri = new Uri(_configuration["ServerUrl"].ToString());
            var creds = new FtpCredentials
                {
                    Username = _configuration["Username"].ToString(),
                    Password = _configuration["Password"].ToString()
                };

            var ftpClient = new FtpClient(uri, creds);
            ftpClient.DownloadFiles();
        }

        public IEnumerator<ISampleValues> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get; private set; }

        public ISampleValues this[int index]
        {
            get { throw new NotImplementedException(); }
        }

        public ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; private set; }
        public DateTime ActualStartTime { get; private set; }
        public DateTime ActualEndTime { get; private set; }
        public DateTime DesiredStartTime { get; set; }
        public DateTime DesiredEndTime { get; set; }
        public ISampleValues GetNearest(DateTime time)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ReadOnlyDictionary<string, double>> ExportStatistics(bool firstRowHeaders = true, string defValue = "missing")
        {
            throw new NotImplementedException();
        }

        public dynamic AnalysisScratchPad { get; private set; }
        public void AddAnalysisNote(AnalysisNote note)
        {
            throw new NotImplementedException();
        }

        public void Analyze(IEnumerable<SetAnalyzer> setAnalyzers, IEnumerable<StatAnalyzer> statAnalyzers)
        {
            throw new NotImplementedException();
        }
    }
}