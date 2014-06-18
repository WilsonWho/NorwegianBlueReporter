using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using NorwegianBlue.Data.Experiment;
using NorwegianBlue.Data.Sample;
using NorwegianBlue.Data.SampleSet;
using NorwegianBlue.Notes.AnalysisNotes;

namespace NorwegianBlue.Data.Experiment
{
    /// <summary>
    /// Composes several data sets together
    /// </summary>
    /// <remarks>
    /// Many fields are aggregates of the contained datasets.
    /// Care needs to be taken to ensure consistent and up-to-date data is provided.
    /// For example, don't re-create Lists, clear and re-add content.
    /// </remarks>
    abstract public class BaseExperimentWithAnalysis : IExperiment<ISampleSetValues<ISampleValues>>, IExperimentAnalysis<ISampleAnalysis>
    {
        private readonly List<ISampleSetAnalysis<ISampleAnalysis>> _sampleSets = new List<ISampleSetAnalysis<ISampleAnalysis>>();

        private readonly List<ISampleAnalysis> _samples = new List<ISampleAnalysis>();

        private readonly dynamic _analysisScratchPad = new ExpandoObject();
        public dynamic AnalysisScratchPad
        {
            get { return _analysisScratchPad; }
        }

        private readonly List<AnalysisNote> _experimentAnalysisNotes = new List<AnalysisNote>();
        private readonly List<AnalysisNote> _analysisNotes = new List<AnalysisNote>(); 
        private ReadOnlyCollection<AnalysisNote> _roAnalysisNotes;
        
        /// <summary>
        /// Aggregation of all the Analysis notes
        /// </summary>
        /// <remarks>
        /// Content has to be updated when any of the aggregate data changes.
        /// </remarks>
        public ReadOnlyCollection<AnalysisNote> AnalysisNotes
        {
            get
            {
                return _roAnalysisNotes ?? (_roAnalysisNotes =
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

        public virtual IEnumerator<ISampleAnalysis> GetEnumerator()
        {
            return _samples.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        ISampleAnalysis IReadOnlyList<ISampleAnalysis>.this[int index]
        {
            get { return _samples[index]; }
        }

        ISampleAnalysis IExperimentValues<ISampleAnalysis>.this[DateTime time]
        {
            get { return SampleSetComparisons<ISampleAnalysis>.GetNearestToTime(_samples, time); }
        }

        ISampleAnalysis IExperimentValues<ISampleAnalysis>.this[DateTime time, TimeSpan tolerance, bool absolute]
        {
            get { return SampleSetComparisons<ISampleAnalysis>.GetNearestToTime(_samples, time, tolerance, absolute); }
        }

        protected BaseExperimentWithAnalysis(IReadOnlyDictionary<object, object> configuration)
        {
            AnalysisScratchPad.ignorableFields = (configuration.ContainsKey("FieldsToIgnore")
                                                     ? configuration["FieldsToIgnore"]
                                                     : new List<string>())    // "FiledsToIgnore" might be absent in the config file
                                                ?? new List<string>();     // FieldsToIgnore might be defined but empty in the config...
        }

        /// <summary>
        /// Ensure the samples list contains all samples
        /// </summary>
        /// <remarks>
        /// Should morph into an event handler- and all other calls should be removed
        /// </remarks>
        private void UpDateSamples()
        {
            _samples.Clear();

            foreach (var sampleSet in _sampleSets)
            {
                foreach (var sample in sampleSet)
                {
                    _samples.Add(sample);
                }
            }
        }

        /// <summary>
        /// Ensure the samples list contains all analysis notes
        /// </summary>
        /// <remarks>
        /// Should morph into an event handler- and all other calls should be removed
        /// </remarks>
        private void UpdateAnalysisNotes()
        {
            _analysisNotes.Clear();
            _analysisNotes.AddRange(_experimentAnalysisNotes);

            foreach (var sampleSet in _sampleSets)
            {

                foreach (var sample in sampleSet)
                {
                    _samples.Add(sample);
                }
            }


        }

        public virtual void AddAnalysisNote(AnalysisNote note)
        {
            _experimentAnalysisNotes.Add(note);
        }


        /// <summary>
        /// Add the sample set and contained samples. Samples will be sorted by time stamp.
        /// </summary>
        /// <remarks>
        /// The sampleSets 
        /// </remarks>
        /// <param name="sampleSet">sample set to add</param>
        public virtual void AddSampleSet(ISampleSetValues<ISampleValues> sampleSet)
        {
            throw new NotImplementedException();
        }

        public void Analyze(List<Tuple<List<SetAnalyzer<ISampleSetAnalysis<ISampleAnalysis>, ISampleAnalysis>>, List<SampleInSetAnalyzer<ISampleSetAnalysis<ISampleAnalysis>, ISampleAnalysis>>>> commonDataSetsAnalysis, List<Tuple<Type, List<SetAnalyzer<ISampleSetAnalysis<ISampleAnalysis>, ISampleAnalysis>>, List<SampleInSetAnalyzer<ISampleSetAnalysis<ISampleAnalysis>, ISampleAnalysis>>>> dataSetsAnalysis, List<ExperimentAnalyzer<IExperimentAnalysis<ISampleAnalysis>, ISampleAnalysis>> experimentAnalysis)
        {
            throw new NotImplementedException();
        }

        public void Analyze(List<Tuple<List<SetAnalyzer<ISampleSetAnalysis<ISampleAnalysis>, ISampleAnalysis>>, List<SampleInSetAnalyzer<ISampleSetAnalysis<ISampleAnalysis>, ISampleAnalysis>>>> commonDataSetsAnalysis, List<Tuple<Type, List<SetAnalyzer<ISampleSetAnalysis<ISampleAnalysis>, ISampleAnalysis>>, List<SampleInSetAnalyzer<ISampleSetAnalysis<ISampleAnalysis>, ISampleAnalysis>>>> dataSetsAnalysis)
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
