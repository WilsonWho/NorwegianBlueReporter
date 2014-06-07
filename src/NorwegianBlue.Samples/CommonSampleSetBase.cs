using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace NorwegianBlue.Samples
{
    public abstract class CommonSampleSetBase<T> : ISampleSet<T>, ISampleSetAnalysis<T> where T: ISampleAnalysis
    {
        private readonly List<T> _samples = new List<T>();

        private readonly dynamic _analysisScratchPad = new ExpandoObject();
        public dynamic AnalysisScratchPad
        {
            get { return _analysisScratchPad; }
        }

        public abstract Type AnalysisNoteType { get; }
        private readonly List<AnalysisNote> _analysisNotes = new List<AnalysisNote>();
        private ReadOnlyCollection<AnalysisNote> _roAnalysisNote;
        public ReadOnlyCollection<AnalysisNote> AnalysisNotes
        {
            get { return _roAnalysisNote ?? ( _roAnalysisNote = 
                                              new ReadOnlyCollection<AnalysisNote>(_analysisNotes)); }
        }

        public int Count { get { return _samples.Count; } }

        public DateTime StartTime
        {
            get
            {
                if (0 == _samples.Count)
                {
                    throw new DataException("No samples in collection");
                }
                return _samples.First().TimeStamp;
            }
        }

        public DateTime EndTime
        {
            get
            {
                if (0 == _samples.Count)
                {
                    throw new DataException("No samples in collection");
                }
                return _samples.Last().TimeStamp;
            }
        }

        T IReadOnlyList<T>.this[int index]
        {
            get { return _samples[index]; }
        }

        T ISampleSetValues<T>.this[DateTime time]
        {
            get { return SampleSetComparisons<T>.GetNearestToTime(_samples, time); }
        }

        T ISampleSetValues<T>.this[DateTime time, TimeSpan tolerance, bool absolute]
        {
            get { return SampleSetComparisons<T>.GetNearestToTime(_samples, time, tolerance, absolute); }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _samples.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        // Split into Parse() and DoParse() to enforce sorting of the dataset after additions 
        public void Parse(TimeZone desiredTimeZone, DateTime? startTime, DateTime? endTime, dynamic dataSourceConfigObject)
        {
            var newSamples = DoParse(desiredTimeZone, startTime, endTime, dataSourceConfigObject);
            _samples.AddRange(newSamples);
            _samples.Sort(new SampleTimeComparer<T>());
        }

        public abstract List<T> DoParse(TimeZone desiredTimeZone, DateTime? startTime, DateTime? endTime, dynamic dataSourceConfigObject);

        //        public void Analyze(IEnumerable<SetAnalyzer<ISampleSetAnalysis<IagoSample>, IagoSample>> setAnalyzers,
        //                            IEnumerable<SampleInSetAnalyzer<ISampleSetAnalysis<IagoSample>, IagoSample>> statAnalyzers)
        public void Analyze(dynamic setAnalyzers, dynamic statAnalyzers)
        {
            foreach (var analyzer in setAnalyzers)
            {
                analyzer.Invoke(this);
            }

            foreach (var stat in _samples)
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
