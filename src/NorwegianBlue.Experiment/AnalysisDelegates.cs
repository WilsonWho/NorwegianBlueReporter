using NorwegianBlue.Samples;

namespace NorwegianBlue.Experiment
{
    // Experiment level analysis
    public delegate void ExperimentAnalyzer<in T1, in T2>(T1 experiment)
        where T1 : IExperimentAnalysis<T2>
        where T2 : ISampleAnalysis;
}