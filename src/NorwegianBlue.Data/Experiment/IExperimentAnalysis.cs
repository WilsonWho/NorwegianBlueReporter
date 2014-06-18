using System;
using System.Collections.Generic;
using NorwegianBlue.Data.Sample;
using NorwegianBlue.Data.SampleSet;
using NorwegianBlue.Notes.AnalysisNotes;

namespace NorwegianBlue.Data.BaseExperimentWIthAnalysis
{
    public interface IExperimentAnalysis<out T> : IExperimentValues<T> where T : ISampleAnalysis
    {
        dynamic AnalysisScratchPad { get; }
        void AddAnalysisNote(AnalysisNote note);

        // An experiment is the outer-most layer, it is responsible for performing the analysis of each set it holds.
        // NB- this doesn't declare how to specify which algorithms to run on the datasets contained.
        //     Assumed to be via the config file.

        void Analyze( List<Tuple<List<SetAnalyzer<ISampleSetAnalysis<ISampleAnalysis>, ISampleAnalysis>>, List<SampleInSetAnalyzer<ISampleSetAnalysis<ISampleAnalysis>, ISampleAnalysis>>>> commonDataSetsAnalysis,

                      List<Tuple<Type, 
                          List<SetAnalyzer<ISampleSetAnalysis<ISampleAnalysis>, ISampleAnalysis>>,
                          List<SampleInSetAnalyzer<ISampleSetAnalysis<ISampleAnalysis>, ISampleAnalysis>>>> dataSetsAnalysis,

                      List<ExperimentAnalyzer<IExperimentAnalysis<ISampleAnalysis>, ISampleAnalysis>> experimentAnalysis
            );
    }
}