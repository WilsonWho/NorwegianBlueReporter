using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NorwegianBlue.Samples;

namespace NorwegianBlue.Experiment
{
    // An Experiment is intended to be a summary collection of various types of analysis.
    // 
    public interface IExperiment<T> where T : ISampleSet<ISampleValues>
    {
        // Parse must ensure that samples are saved in time-stamp sorted order.
        void Parse(TimeZone desiredTimeZone, DateTime? startTime, DateTime? endTime, dynamic configObject);
    }

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

    public interface IExperimentAnalysis<out T> : IExperimentValues<T> where T : ISampleAnalysis
    {
        dynamic AnalysisScratchPad { get; }
        void AddAnalysisNote(AnalysisNote note);

        // As an experiment is the very 
        void Analyze();
    }
}
