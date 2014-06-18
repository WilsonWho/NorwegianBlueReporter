using NorwegianBlue.Data.Sample;

namespace NorwegianBlue.Data.Experiment
{

    public delegate void ExperimentAnalyzer<in T1, in T2>(T1 experiment)
        where T1 : IExperimentAnalysis<T2>
        where T2 : ISampleAnalysis;


}
