using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Samples
{
    public abstract class CommonSampleSetBase<T> : ISampleSet<T>, ISampleSetAnalysis<T> where T: ISampleAnalysis
    {
        private readonly List<T> _samples = new List<T>();

        private readonly List<AnalysisNote> _analysisNotes = new List<AnalysisNote>();
        private readonly dynamic _analysisScratchPad = new ExpandoObject();
        public dynamic AnalysisScratchPad
        {
            get { return _analysisScratchPad; }
        }

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

        protected CommonSampleSetBase() 
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
                    var newStat = new T();
                    newStat.Parse(timeZone, line);
                    _samples.Add(newStat);
                }
            }
            _samples.Sort(new SampleTimeComparer());
        }

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

