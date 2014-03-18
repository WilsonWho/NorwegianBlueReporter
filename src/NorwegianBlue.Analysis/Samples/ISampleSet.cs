using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NorwegianBlue.Analysis.Samples
{
    public interface ISampleSet
    {
        void Parse(TimeZone timeZone, string dataLocation, DateTime? startTime, DateTime? endTime);
    }

    public interface ISampleSetValues : IReadOnlyList<ISampleValues>
    {
        ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; }
        DateTime ActualStartTime { get; }
        DateTime ActualEndTime { get; }
        DateTime DesiredStartTime { get; set; }
        DateTime DesiredEndTime { get; set; }
        ISampleValues GetNearest(DateTime time);
        // return set of values, with any missing values geting the specified default value.
        ReadOnlyCollection<ReadOnlyDictionary<string, double>> ExportStatistics(bool firstRowHeaders = true, string defValue = "missing");
    }

    public interface ISampleSetAnalysis : ISampleSetValues
    {
        dynamic AnalysisScratchPad { get; }
        void AddAnalysisNote(AnalysisNote note);
        void Analyze(IEnumerable<SetAnalyzer> setAnalyzers, IEnumerable<StatAnalyzer> statAnalyzers);
    }
}
