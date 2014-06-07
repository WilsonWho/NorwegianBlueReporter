using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NorwegianBlue.Samples;

namespace NorwegianBlue.Experiment
{
    // An Experiment is intended to be a summary collection of various types of analysis.
    // 
    public interface IExperiment<in T> where T : ISampleSetValues<ISampleValues>
    {
        void AddSampleSet(T sampleSet);
    }

    public interface IExperimentValues<out T> : IReadOnlyList<T> where T : ISampleValues
    {
        T this[DateTime time] { get; }
        T this[DateTime time, TimeSpan tolerance, bool absolute] { get; }

        ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; }
        DateTime StartTime { get; }
        DateTime EndTime { get; }

        // return set of values, with any missing values geting the specified default value.
        ReadOnlyCollection<ReadOnlyDictionary<string, double>> ExportStatistics(bool firstRowHeaders = true,
                                                                                string defValue = "missing");
    }

    public interface IExperimentAnalysis<out T> : IExperimentValues<T> where T : ISampleAnalysis
    {
        dynamic AnalysisScratchPad { get; }
        void AddAnalysisNote(AnalysisNote note);

        // An experiment is the outer-most layer, it is responsible for performing the analysis of each set it holds.
        // NB- this doesn't declare how to specify which algorithms to run on the datasets contained.
        //     Assumed to be via the config file.

        void Analyze( List<Tuple<List<SetAnalyzer<ISampleSetAnalysis<ISampleAnalysis>, ISampleAnalysis>>,
                                 List<SampleInSetAnalyzer<ISampleSetAnalysis<ISampleAnalysis>, ISampleAnalysis>>>> commonDataSetsAnalysis,

                      List<Tuple<Type, 
                                 List<SetAnalyzer<ISampleSetAnalysis<ISampleAnalysis>, ISampleAnalysis>>,
                                 List<SampleInSetAnalyzer<ISampleSetAnalysis<ISampleAnalysis>, ISampleAnalysis>>>> dataSetsAnalysis,

                      List<ExperimentAnalyzer<IExperimentAnalysis<ISampleAnalysis>, ISampleAnalysis>> experimentAnalysis
            );
    }
}
