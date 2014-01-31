using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsReader
{
    interface IStatisticsValues
    {
        DateTime TimeStamp { get; }
        ReadOnlyDictionary<String,String> Stats {get;}
        ReadOnlyDictionary<String, String> AnalysisNotes { get; } 
    }

    interface IStatistics
    {
        void Parse(String input);
    }

    interface IStatisticsAddNote
    {
        void AddAnalysisNote(String analysisName, String note);
    }

    interface IStatisticsSelfAnalysis
    {
        void Analyze();
    }


}
