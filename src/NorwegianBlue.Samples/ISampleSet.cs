using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NorwegianBlue.Samples
{
// ReSharper disable UnusedTypeParameter
// Although the type is not used directly, it's assumed at some point ISampleSetValues will be implemented, which both derives
// from this interface and has this requirement.
    public interface ISampleSet<T> where T: ISampleValues
// ReSharper restore UnusedTypeParameter
    {
        // Parse must ensure that samples are saved in time-stamp sorted order.
        void Parse(TimeZone desiredTimeZone, DateTime? startTime, DateTime? endTime, dynamic configObject);
    }

    public interface ISampleSetValues<out T> : IReadOnlyList<T> where T: ISampleValues
    {
        T this[DateTime time] { get; }
        T this[DateTime time, TimeSpan tolerance, bool absolute] { get; }

        // needed so reporting can figure out what generated a note, to be able to do grouping, sorting, etc
        Type AnalysisNoteType { get; }
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
        //                     IEnumerable<SampleInSetAnalyzer<ISampleSetAnalysis<T>, T>> statAnalyzers);

        void Analyze(dynamic setAnalyzers, dynamic statAnalyzers);
    }
}
