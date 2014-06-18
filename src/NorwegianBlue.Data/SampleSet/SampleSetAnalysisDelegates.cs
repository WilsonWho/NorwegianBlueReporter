using NorwegianBlue.Data.Sample;

namespace NorwegianBlue.Data.SampleSet
{
    // should only add notes to the set; applied from a set
    public delegate void SetAnalyzer<in T1, in T2>(T1 sampleSet)
        where T1 : ISampleSetAnalysis<T2>
        where T2 : ISampleAnalysis;

    // should only add notes to samples, but needs values from the set analysis; applied from a set
    public delegate void SampleInSetAnalyzer<in T1, in T2>(T1 sampleSet, T2 sample)
        where T1 : ISampleSetAnalysis<T2>
        where T2 : ISampleAnalysis;
}
