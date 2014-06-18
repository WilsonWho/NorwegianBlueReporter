using System.Collections.Generic;
using NorwegianBlue.Notes.AnalysisNotes;

namespace NorwegianBlue.Data.Sample
{
    public interface ISampleAnalysis : ISampleValues
    {
        dynamic AnalysisScratchPad { get; }
        void AddAnalysisNote(AnalysisNote note);
        void Analyze(IEnumerable<SampleAnalyzer<ISampleAnalysis>> analyzers);
    }
}
