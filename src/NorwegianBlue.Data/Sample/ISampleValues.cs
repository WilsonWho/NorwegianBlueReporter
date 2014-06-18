using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NorwegianBlue.Notes.AnalysisNotes;

namespace NorwegianBlue.Data.Sample
{
    public interface ISampleValues:IReadOnlyDictionary<string, double>
    {
        DateTime TimeStamp { get; }
        ReadOnlyDictionary<string, Tuple<bool,string>> NonStats { get; }
        Type AnalysisNoteType { get; }
        ReadOnlyCollection<AnalysisNote> AnalysisNotes { get; }

        // normal dictionary methods will exclude any ignored statistic fields.
        // the following allow fuller access control to the stored statistic fields.

        int CountFiltered(bool includeIgnored, List<string> extraIgnores);
        IEnumerator<KeyValuePair<string, double>> GetEnumerator(bool includeIgnored, IEnumerable<string> extraIgnores);
        bool ContainsKey(string key, bool includeIgnored, IEnumerable<string> extraIgnores);
        bool TryGetValue(string key, out double value, bool includeIgnored, IEnumerable<string> extraIgnores);

        double this[string key, bool includeIgnored = false, IEnumerable<string> extraIgnores = null] { get; }
        IEnumerable<string> FilteredKeys(bool includeIgnored, IEnumerable<string> extraIgnores);
        IEnumerable<double> FilteredValues(bool includeIgnored, IEnumerable<string> extraIgnores);
    }
}