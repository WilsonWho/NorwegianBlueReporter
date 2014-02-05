using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;

namespace StatsReader
{

    public interface IStatisticsSet
    {
        void Parse(TextReader input);
    }

    public interface IStatisticsSetValues
    {
        ReadOnlyCollection<IStatisticsValues> Statistics { get; }
        ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; }
    }

    public interface IStatisticsSetAnalysis : IStatisticsSetValues
    {
        dynamic AnalysisScratchPad { get; }
        void AddAnalysisNote(AnalysisNote note);
        void Analyze(IEnumerable<SetAnalyzer> setAnalyzers, IEnumerable<StatAnalyzer> statAnalyzers);
    }
}
