using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using NorwegianBlue.Samples;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Experiment
{
    public class Experiment : IExperiment<ISampleSetValues<ISampleValues>>, IExperimentAnalysis<ISampleValues>
    {

        private readonly List<ISampleValues> _samples = new List<ISampleValues>() ;

        private readonly dynamic _analysisScratchPad = new ExpandoObject();
        public virtual dynamic AnalysisScratchPad
        {
            get { return _analysisScratchPad; }
        }

        private readonly List<AnalysisNote> _analysisNotes = new List<AnalysisNote>();
        private ReadOnlyCollection<AnalysisNote> _roAnalysisNote;
        public ReadOnlyCollection<AnalysisNote> AnalysisNotes
        {
            get
            {
                return _roAnalysisNote ?? (_roAnalysisNote =
                                            new ReadOnlyCollection<AnalysisNote>(_analysisNotes));
            }
        }

        public virtual int Count
        {
            get { return _samples.Count; }
        }

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

        public virtual IEnumerator<ISampleValues> GetEnumerator()
        {
            return _samples.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        ISampleValues IReadOnlyList<ISampleValues>.this[int index]
        {
            get { return _samples[index]; }
        }

        ISampleValues IExperimentValues<ISampleValues>.this[DateTime time]
        {
            get { return SampleSetComparisons<ISampleValues>.GetNearestToTime(_samples, time); }
        }

        ISampleValues IExperimentValues<ISampleValues>.this[DateTime time, TimeSpan tolerance, bool absolute]
        {
            get { return SampleSetComparisons<ISampleValues>.GetNearestToTime(_samples, time, tolerance, absolute); }
        }

        public Experiment()
        {
            Dictionary<object, object> configuration = YamlParser.GetConfiguration();
        }

        public virtual void AddSampleSet(ISampleSetValues<ISampleValues> sampleSet)
        {
            throw new NotImplementedException();
        }

        public virtual void AddAnalysisNote(AnalysisNote note)
        {
            throw new NotImplementedException();
        }

        public virtual void Analyze()
        {
            throw new NotImplementedException();
        }

        public virtual ReadOnlyCollection<ReadOnlyDictionary<string, double>> ExportStatistics(bool firstRowHeaders = true, string defValue = "missing")
        {
            throw new NotImplementedException();
        }
    }
}
