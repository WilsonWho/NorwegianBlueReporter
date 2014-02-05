using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace StatsReader
{
    public interface IStatistics
    {
        void Parse(String input);
    }

    public interface IStatisticsValues
    {
        DateTime TimeStamp { get; }
        ReadOnlyDictionary<String,Double> Stats {get;}
        ReadOnlyDictionary<String, String> NonStats { get; }
        ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; } 
    }

    public interface IStatisticsAnalysis : IStatisticsValues
    {
        dynamic AnalysisScratchPad { get; }
        void AddAnalysisNote(AnalysisNote note);
        void Analyze(IEnumerable<StatAnalyzer> analyzers);
    }
}
