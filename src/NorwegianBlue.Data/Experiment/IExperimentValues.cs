using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NorwegianBlue.Data.Sample;
using NorwegianBlue.Notes.AnalysisNotes;

namespace NorwegianBlue.Data.BaseExperimentWIthAnalysis
{
    public interface IExperimentValues<out T> : IReadOnlyList<T> where T : ISampleValues
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
}