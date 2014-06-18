using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NorwegianBlue.Data.Sample;
using NorwegianBlue.Notes.AnalysisNotes;

namespace NorwegianBlue.Data.SampleSet
{
    public interface ISampleSetValues<out T> : IReadOnlyList<T> where T: ISampleValues
    {
        T this[DateTime time] { get; }
        T this[DateTime time, TimeSpan tolerance, bool absolute] { get; }

        // needed so reporting can figure out what generated a note, to be able to do grouping, sorting, etc
        Type AnalysisNoteType { get; }
        ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; }
        DateTime StartTime { get; }
        DateTime EndTime { get; }

        // Parse must ensure that samples are saved in time-stamp sorted order.
        void Parse(TimeZone desiredTimeZone, DateTime? startTime, DateTime? endTime, dynamic configObject);

        // return set of values, with any missing values geting the specified default value.
        ReadOnlyCollection<ReadOnlyDictionary<string, double>> ExportStatistics(bool firstRowHeaders = true, 
                                                                                string defValue = "missing");
    }
}