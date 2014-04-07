namespace NorwegianBlue.Samples
{
    // thought: SampleSets have access to the individual samples, but samples only have access to themselves.
    // Thus analyzing individual samples happens AFTER analyzing the set, and may require information from
    // the analysis of the set.
    // Also: using an expando object in the samples and sets to store analysis results and 
    // temporary values- to allow free form, low-overhead, dependencies between different statistical
    // analysis.  If repeating patterns reveal themselves this might be refactored into "proper" interfaces.

    // should only add notes to the set; applied from a set
    public delegate void SetAnalyzer<in T1, in T2>(T1 sampleSet) 
                                            where T1 : ISampleSetAnalysis<T2> where T2: ISampleAnalysis;
    
    // should only add notes to samples, but needs values from the set analysis; applied from a set
    public delegate void SampleInSetAnalyzer<in T1, in T2>(T1 sampleSet, T2 sample) 
                                            where T1 : ISampleSetAnalysis<T2> where T2 : ISampleAnalysis;

    // should only add notes to samples, only uses values from the sample; applied from a sample
    public delegate void SampleAnalyzer<in T>(T sample) where T : ISampleAnalysis;

}