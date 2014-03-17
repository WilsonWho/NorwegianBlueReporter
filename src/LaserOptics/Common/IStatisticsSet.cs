using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LaserOptics.IagoStats;

namespace LaserOptics.Common
{
    public interface IStatisticsSet
    {
        void Parse(TimeZone timeZone, string dataLocation, DateTime? startTime, DateTime? endTime);
    }

    public interface IStatisticsSetValues : IReadOnlyList<IStatisticsValues>
    {
        ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; }
        DateTime ActualStartTime { get; }
        DateTime ActualEndTime { get; }
        DateTime DesiredStartTime { get; set; }
        DateTime DesiredEndTime { get; set; }
        IStatisticsValues GetNearest(DateTime time);
        // return set of values, with any missing values geting the specified default value.
        ReadOnlyCollection<ReadOnlyDictionary<string, double>> ExportStatistics(bool firstRowHeaders = true, string defValue = "missing");
    }

    public interface IStatisticsSetAnalysis : IStatisticsSetValues
    {
        dynamic AnalysisScratchPad { get; }
        void AddAnalysisNote(AnalysisNote note);
        void Analyze(IEnumerable<SetAnalyzer> setAnalyzers, IEnumerable<StatAnalyzer> statAnalyzers);
    }
}
