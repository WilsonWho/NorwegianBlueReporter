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
        //TODO: refactor to return iterators

        ReadOnlyCollection<IStatisticsValues> Statistics { get; }
        ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; }
        // return set of values, with any missing values geting the specified default value.
        ReadOnlyCollection<ReadOnlyDictionary<string, double>> ExportStatistics(bool firstRowHeaders=true, string defValue = "missing");
    }

    public interface IStatisticsSetAnalysis : IStatisticsSetValues
    {
        dynamic AnalysisScratchPad { get; }
        void AddAnalysisNote(AnalysisNote note);
        void Analyze(IEnumerable<SetAnalyzer> setAnalyzers, IEnumerable<StatAnalyzer> statAnalyzers);
    }
}
