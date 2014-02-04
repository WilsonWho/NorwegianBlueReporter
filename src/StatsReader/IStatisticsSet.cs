using System.Collections.ObjectModel;
using System.IO;

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

    public interface IStatisticsSetAddAnalysis
    {
        dynamic AnalysisScratchPad { get; }
        void AddAnalysisNote(AnalysisNote note);
        void AddAnalysisNoteToStats(IStatisticsValues statistic, AnalysisNote note);
    }

    public interface IStatisticsSetSelfAnalysis
    {
        void Analyze();

    }

}
