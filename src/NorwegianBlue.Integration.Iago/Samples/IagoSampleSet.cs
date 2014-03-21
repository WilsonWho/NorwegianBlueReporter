using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using NorwegianBlue.Analysis;
using NorwegianBlue.Analysis.Samples;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.IagoIntegration.Samples
{
    public class IagoSampleSet : ISampleSet, ISampleSetAnalysis
    {
        private readonly dynamic _analysisScratchPad = new ExpandoObject();
        private readonly List<IagoSample> _iagoSamples = new List<IagoSample>();
        private readonly List<AnalysisNote> _analysisNotes = new List<AnalysisNote>();
        private ReadOnlyCollection<AnalysisNote> _roAnalysisNote;

        public ReadOnlyCollection<AnalysisNote> AnalysisNotes
        {
            get { return _roAnalysisNote ?? (_roAnalysisNote = new ReadOnlyCollection<AnalysisNote>(_analysisNotes)); }
        }

        public DateTime ActualStartTime { get; private set; }
        public DateTime ActualEndTime { get; private set; }
        public DateTime DesiredStartTime { get; set; }
        public DateTime DesiredEndTime { get; set; }

        public dynamic AnalysisScratchPad
        {
            get { return _analysisScratchPad; }
        }

        public IagoSampleSet()
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
        }

        public void Parse(TimeZone timeZone, string dataLocation, DateTime? startTime, DateTime? endTime)
        {
            using (var input = File.OpenText(dataLocation))
            {
                string line;
                while ((line = input.ReadLine()) != null)
                {
                    var newStat = new IagoSample();
                    newStat.Parse(line);
                    _iagoSamples.Add(newStat);
                }
            }

           // TODO:  _iagoSamples.Sort();
        }

        public ISampleValues GetNearest(DateTime time)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<ReadOnlyDictionary<string, double>> ExportStatistics(bool firstRowHeaders = true,
                                                                                       string defValue = "missing")
        {
            throw new NotImplementedException();
        }

        public void Analyze(IEnumerable<SetAnalyzer> setAnalyzers, IEnumerable<StatAnalyzer> statAnalyzers)
        {
            foreach (var analyzer in setAnalyzers)
            {
                analyzer.Invoke(this);
            }

            foreach (var stat in _iagoSamples)
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
    }
}