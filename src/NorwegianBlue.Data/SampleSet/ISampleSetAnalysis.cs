using NorwegianBlue.Data.Sample;
using NorwegianBlue.Notes.AnalysisNotes;

namespace NorwegianBlue.Data.SampleSet
{
    public interface ISampleSetAnalysis<out T> : ISampleSetValues<T> where T: ISampleAnalysis
    {
        dynamic AnalysisScratchPad { get; }
        void AddAnalysisNote(AnalysisNote note);

        // This didn't work- couldn't take delegates taking items with more specific (more derived) interfaces(!)
        //        void Analyze(IEnumerable<SetAnalyzer<ISampleSetAnalysis<T>, T>> setAnalyzers, 
        //                     IEnumerable<SampleInSetAnalyzer<ISampleSetAnalysis<T>, T>> statAnalyzers);

        void Analyze(dynamic setAnalyzers, dynamic statAnalyzers);
    }
}