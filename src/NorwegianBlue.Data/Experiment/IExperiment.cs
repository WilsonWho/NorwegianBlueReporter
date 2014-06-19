using NorwegianBlue.Data.Sample;
using NorwegianBlue.Data.SampleSet;

namespace NorwegianBlue.Data.Experiment
{
    // An AbstractBaseExperimentWithAnalysis is intended to be a summary collection of various types of analysis.
    // 
    public interface IExperiment<in T> where T : ISampleSetValues<ISampleValues>
    {
        void AddSampleSet(T sampleSet);
    }
}
