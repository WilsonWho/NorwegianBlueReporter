using System;
using System.Collections.ObjectModel;

namespace StatsReader
{
    public interface IStatisticsValues
    {
        DateTime TimeStamp { get; }
        ReadOnlyDictionary<String,String> Stats {get;}
        ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; } 
    }

    public interface IStatistics
    {
        void Parse(String input);
    }

    public interface IStatisticsAddAnalysis
    {
        dynamic AnalysisScratchPad { get; }
        void AddAnalysisNote(AnalysisNote note);
    }

    public interface IStatisticsSelfAnalysis
    {
        void Analyze();
    }


}
