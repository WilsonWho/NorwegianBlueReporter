using NorwegianBlue.Analysis.Samples;

namespace NorwegianBlue.Analysis
{
    // thought: StatsSets have access to the individual stats, but stats only have access to themselves.
    // Thus analyzing individual stats happens AFTER analyzing the set, and may require information from
    // the analysis of the set.
    // Also: usign an expando object in the stat sets and stats to store analysis results and 
    // temporary values- to allow free form, low-overhead, dependencies between different statistical
    // analysis.  If repeating patterns reveal themselves this might be refactored into "proper" interfaces.
    public delegate void SetAnalyzer(ISampleSetAnalysis statSet);
    public delegate void StatAnalyzer(ISampleSetAnalysis statSet, ISampleAnalysis stat);
}