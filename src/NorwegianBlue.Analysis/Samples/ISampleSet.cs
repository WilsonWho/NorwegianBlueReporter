using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NorwegianBlue.Analysis.Samples
{
    public interface ISampleSet<T> where T: ISampleValues
    {
        // Parse must ensure that samples are saved in time-stamp sorted order.
        void Parse(TimeZone timeZone, string dataLocation, DateTime? startTime, DateTime? endTime);
    }

    public interface ISampleSetValues<out T> : IReadOnlyList<T> where T: ISampleValues
    {
        T this[DateTime time] { get; }
        T this[DateTime time, TimeSpan tolerance, bool absolute] { get; }

        ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; }
        DateTime StartTime { get; }
        DateTime EndTime { get; }

        // return set of values, with any missing values geting the specified default value.
        ReadOnlyCollection<ReadOnlyDictionary<string, double>> ExportStatistics(bool firstRowHeaders = true, 
                                                                                string defValue = "missing");
    }

    public interface ISampleSetAnalysis<out T> : ISampleSetValues<T> where T: ISampleAnalysis
    {
        dynamic AnalysisScratchPad { get; }
        void AddAnalysisNote(AnalysisNote note);

        // This didn't work- couldn't take delegates taking items with more specific (more derived) interfaces(!)
        //        void Analyze(IEnumerable<SetAnalyzer<ISampleSetAnalysis<T>, T>> setAnalyzers, 
        //                     IEnumerable<StatAnalyzer<ISampleSetAnalysis<T>, T>> statAnalyzers);

        void Analyze(dynamic setAnalyzers, dynamic statAnalyzers);
    }
}
