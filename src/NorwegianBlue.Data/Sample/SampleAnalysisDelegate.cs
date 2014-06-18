namespace NorwegianBlue.Data.Sample
{
    // should only add notes to samples, only uses values from the sample; applied from a sample
    public delegate void SampleAnalyzer<in T>(T sample) where T : ISampleAnalysis;
}
