using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LaserOptics
{
    public interface IStatistics
    {
        void Parse(String input);
    }

    public interface IStatisticsValues
    {
        DateTime TimeStamp { get; }
        ReadOnlyDictionary<string,double> Stats {get;}
        ReadOnlyDictionary<string, string> NonStats { get; }
        ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; } 
    }

    public interface IStatisticsAnalysis : IStatisticsValues
    {
        dynamic AnalysisScratchPad { get; }
        void AddAnalysisNote(AnalysisNote note);
        void Analyze(IEnumerable<StatAnalyzer> analyzers);
    }
}
