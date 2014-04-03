using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using NorwegianBlue.Analysis;
using NorwegianBlue.Analysis.Samples;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.IagoIntegration.Samples
{
    public class IagoSampleSet : ISampleSet<IagoSample>, ISampleSetAnalysis<IagoSample>
    {
        private readonly List<IagoSample> _iagoSamples = new List<IagoSample>();

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

        public int Count { get { return _iagoSamples.Count; } }

        public DateTime StartTime {
            get
            {
                if (0 == _iagoSamples.Count)
                {
                    throw new DataException("No samples in collection");
                }
                return _iagoSamples.First().TimeStamp;
            }
        }

        public DateTime EndTime
        {
            get
            {
                if (0 == _iagoSamples.Count)
                {
                    throw new DataException("No samples in collection");
                }
                return _iagoSamples.Last().TimeStamp;
            }
        }

        IagoSample IReadOnlyList<IagoSample>.this[int index]
        {
            get { return _iagoSamples[index]; }
        }

        IagoSample ISampleSetValues<IagoSample>.this[DateTime time]
        {
            get { return SampleSetComparisons<IagoSample>.GetNearestToTime(_iagoSamples, time); }
        }

        IagoSample ISampleSetValues<IagoSample>.this[DateTime time, TimeSpan tolerance, bool absolute]
        {
            get { return SampleSetComparisons<IagoSample>.GetNearestToTime(_iagoSamples, time, tolerance, absolute); }
        }

        IEnumerator<IagoSample> IEnumerable<IagoSample>.GetEnumerator()
        {
            return _iagoSamples.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<IagoSample>)this).GetEnumerator();
        }

        public IagoSampleSet()
        {
            Dictionary<object, object> configuration = YamlParser.GetConfiguration();

            AnalysisScratchPad.ignorableFields = configuration.ContainsKey("FieldsToIgnore") ? configuration["FieldsToIgnore"] : new List<string>();
        }

        public void Parse(TimeZone timeZone, string dataLocation, DateTime? startTime, DateTime? endTime)
        {
            using (var input = File.OpenText(dataLocation))
            {
                string line;
                while ((line = input.ReadLine()) != null)
                {
                    var newStat = new IagoSample();
                    newStat.Parse(timeZone, line);
                    _iagoSamples.Add(newStat);
                }
            }
            _iagoSamples.Sort(new SampleTimeComparer());
        }

//        public void Analyze(IEnumerable<SetAnalyzer<ISampleSetAnalysis<IagoSample>, IagoSample>> setAnalyzers,
//                            IEnumerable<StatAnalyzer<ISampleSetAnalysis<IagoSample>, IagoSample>> statAnalyzers)

        public void Analyze(dynamic setAnalyzers, dynamic statAnalyzers)
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

        public ReadOnlyCollection<ReadOnlyDictionary<string, double>> ExportStatistics(bool firstRowHeaders = true, string defValue = "missing")
        {
            throw new NotImplementedException();
        }
    }
}