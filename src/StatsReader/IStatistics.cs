using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
