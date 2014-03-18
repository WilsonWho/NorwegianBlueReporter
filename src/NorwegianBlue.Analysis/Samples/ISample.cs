using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NorwegianBlue.Analysis.Samples
{
    public interface ISampleValues:IReadOnlyDictionary<String, double>
    {
        DateTime TimeStamp { get; }
        ReadOnlyDictionary<string, string> NonStats { get; }
        ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; }

        // normal dictionary methods will exclude any ignored statistic fields.
        // the following allow fuller access control to the stored statistic fields.

        int CountFiltered(bool includeIgnored, List<string> extraIgnores);
        IEnumerator<KeyValuePair<string, double>> GetEnumerator(bool includeIgnored, IEnumerable<string> extraIgnores);
        bool ContainsKey(string key, bool includeIgnored, IEnumerable<string> extraIgnores);
        bool TryGetValue(string key, out double value, bool includeIgnored, IEnumerable<string> extraIgnores);

        double this[string key, bool includeIgnored = false, List<string> extraIgnores = null] { get; }
        IEnumerable<string> FilteredKeys(bool includeIgnored, IEnumerable<string> extraIgnores);
        IEnumerable<double> FilteredValues(bool includeIgnored, IEnumerable<string> extraIgnores);
    }

    public interface ISampleAnalysis : ISampleValues
    {
        dynamic AnalysisScratchPad { get; }
        void AddAnalysisNote(AnalysisNote note);
        void Analyze(IEnumerable<StatAnalyzer> analyzers);
    }
}
