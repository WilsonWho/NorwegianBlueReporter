using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using LaserOptics.Common;
using LaserYaml;

namespace LaserOptics.IagoStats
{
    public class IagoStatisticsSet : IStatisticsSet, IStatisticsSetAnalysis
    {
        private readonly string _inputFile;
        private readonly dynamic _analysisScratchPad = new ExpandoObject();
        private readonly List<IagoStatistics> _iagoStatistics = new List<IagoStatistics>();
        private readonly List<AnalysisNote> _analysisNotes = new List<AnalysisNote>();
        private ReadOnlyCollection<AnalysisNote> _roAnalysisNote;

        public ReadOnlyCollection<AnalysisNote> AnalysisNotes
        {
            get { return _roAnalysisNote ?? ( _roAnalysisNote = new ReadOnlyCollection<AnalysisNote>(_analysisNotes)); }
        }

        public DateTime ActualStartTime { get; private set; }
        public DateTime ActualEndTime { get; private set; }
        public DateTime DesiredStartTime { get; set; }
        public DateTime DesiredEndTime { get; set; }

        public dynamic AnalysisScratchPad { get { return _analysisScratchPad; } }

        public IagoStatisticsSet()
        {
            Dictionary<object, object> configuration = YamlParser.GetConfiguration();

            if (configuration.ContainsKey("FieldsToIgnore"))
            {
                AnalysisScratchPad.ignorableFields = configuration["FieldsToIgnore"];
            }
            else
            {
                AnalysisScratchPad.ignorableFields = new List<string>();
            }

            _inputFile = configuration["InputFile"].ToString();
        }

        public void Parse()
        {
            using (var input = File.OpenText(_inputFile))
            {
                string line;
                while ((line = input.ReadLine()) != null)
                {
                    var newStat = new IagoStatistics();
                    newStat.Parse(line);
                    _iagoStatistics.Add(newStat);
                }
            }
        }

        public void Parse(TimeZone timeZone, string dataLocation, DateTime? startTime, DateTime? endTime)
        {
            throw new NotImplementedException();
        }

        public IStatisticsValues GetNearest(DateTime time)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ReadOnlyDictionary<string, double>> ExportStatistics(bool firstRowHeaders = true, string defValue = "missing")
        {
            throw new NotImplementedException();
        }

        public void Analyze(IEnumerable<SetAnalyzer> setAnalyzers , IEnumerable<StatAnalyzer> statAnalyzers)
        {
            foreach (var analyzer in setAnalyzers)
            {
                analyzer.Invoke(this);
            }

            foreach (var stat in _iagoStatistics)
            {
                foreach (var analyzer in statAnalyzers)
                {
                    analyzer.Invoke(this, stat);
                }
            }
        }

        public void AddAnalysisNote(AnalysisNote note)
        {
            _analysisNotes.Add(note);
        }

        public IEnumerator<IStatisticsValues> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(IStatisticsValues item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(IStatisticsValues item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(IStatisticsValues[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(IStatisticsValues item)
        {
            throw new NotImplementedException();
        }

        public int Count { get; private set; }
        public bool IsReadOnly { get; private set; }
        public int IndexOf(IStatisticsValues item)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, IStatisticsValues item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public IStatisticsValues this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void Parse(string dataLocation = null, DateTime? startTime = null, DateTime? endTime = null, TimeZone timeZone = null)
        {
            throw new NotImplementedException();
        }
    }

}