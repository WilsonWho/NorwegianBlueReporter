using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using NorwegianBlue.Analysis;
using NorwegianBlue.Analysis.Samples;
using NorwegianBlue.Util.Configuration;
using NorwegianBlue.Util.Web;

namespace NorwegianBlue.Integration.IIS.Samples
{
    public class IisSampleSet : ISampleSet<IisSample>, ISampleSetAnalysis<IisSample>
    {
        private readonly IDictionary<object, object> _configuration;
        private readonly List<IisSample> _iisSamples = new List<IisSample>();

        private readonly List<AnalysisNote> _analysisNotes = new List<AnalysisNote>();
        private readonly dynamic _analysisScratchPad = new ExpandoObject();
        public dynamic AnalysisScratchPad
        {
            get { return _analysisScratchPad; }
        }

        private ReadOnlyCollection<AnalysisNote> _roAnalysisNote;
        public ReadOnlyCollection<AnalysisNote> AnalysisNotes
        {
            get { return _roAnalysisNote ?? (_roAnalysisNote = new ReadOnlyCollection<AnalysisNote>(_analysisNotes)); }
        }

        public int Count { get { return _iisSamples.Count; } }

        public DateTime StartTime
        {
            get
            {
                if (0 == _iisSamples.Count)
                {
                    throw new DataException("No samples in collection");
                }
                return _iisSamples.First().TimeStamp;
            }
        }

        public DateTime EndTime
        {
            get
            {
                if (0 == _iisSamples.Count)
                {
                    throw new DataException("No samples in collection");
                }
                return _iisSamples.Last().TimeStamp;
            }
        }

        IisSample IReadOnlyList<IisSample>.this[int index]
        {
            get { return _iisSamples[index]; }
        }

        IisSample ISampleSetValues<IisSample>.this[DateTime time]
        {
            get { return SampleSetComparisons<IisSample>.GetNearestToTime(_iisSamples, time); }
        }

        IisSample ISampleSetValues<IisSample>.this[DateTime time, TimeSpan tolerance, bool absolute]
        {
            get { return SampleSetComparisons<IisSample>.GetNearestToTime(_iisSamples, time, tolerance, absolute); }
        }

        IEnumerator<IisSample> IEnumerable<IisSample>.GetEnumerator()
        {
            return _iisSamples.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<IisSample>)this).GetEnumerator();
        }

        public IisSampleSet()
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
                var iisSample = new IisSample();
                iisSample.Parse(log, headers);

                _iisSamples.Add(iisSample);
            }
        }

//        public void Analyze(IEnumerable<SetAnalyzer<ISampleSetAnalysis<IisSample>, IisSample>> setAnalyzers, 
//                            IEnumerable<StatAnalyzer<ISampleSetAnalysis<IisSample>, IisSample>> statAnalyzers)
        public void Analyze(dynamic setAnalyzers, dynamic statAnalyzers)
        {
            foreach (var analyzer in setAnalyzers)
            {
                analyzer.Invoke(this);
            }

            foreach (var stat in _iisSamples)
            {
                foreach (var analyzer in statAnalyzers)
                {
                    analyzer.Invoke(this, stat);
                }
            }
        }

        void ISampleSetAnalysis<IisSample>.AddAnalysisNote(AnalysisNote note)
        {
            _analysisNotes.Add(note);
        }

        public ReadOnlyCollection<ReadOnlyDictionary<string, double>> ExportStatistics(bool firstRowHeaders = true, string defValue = "missing")
        {
            throw new NotImplementedException();
        }
    }
}