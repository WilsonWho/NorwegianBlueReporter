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
        private readonly IDictionary<object, object> _configuration;
        private readonly List<IISSample> _iisStatistics = new List<IISSample>();

        public IISSampleSet()
        {
            _configuration = YamlParser.GetConfiguration();
        }

        public void Parse(TimeZone timeZone, string dataLocation, DateTime? startTime, DateTime? endTime)
        {
            var url = _configuration["ServerUrl"].ToString();
            var configCredentials = (IDictionary<object, object>) _configuration["Credentials"];
            var creds = new FtpCredentials
                {
                    Username = configCredentials["Username"].ToString(),
                    Password = configCredentials["Password"].ToString()
                };

            var ftpClient = new FtpClient(url, creds);
            ftpClient.Connect();
            ftpClient.ChangeWorkingDirectory(_configuration["WorkingDirectory"].ToString());
            var localSaveFiles = ftpClient.PullLogFiles();

            var searchParameters = new IISLogSearchParameters
                {
                    StartTime = startTime,
                    EndTime = endTime
                };

            var fileProbe = new IISLogFileProbe(searchParameters);
            var logs = fileProbe.CollectLogsFromTimeInterval(localSaveFiles);

            var headers = logs[0].Split(null);

            for (int index = 1; index < logs.Count; index++)
            {
                var log = logs[index];
                var iisSample = new IISSample();
                iisSample.Parse(log, headers);

                _iisStatistics.Add(iisSample);
            }
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