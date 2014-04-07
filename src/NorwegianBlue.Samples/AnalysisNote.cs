using System.Collections.Generic;
using OxyPlot;

namespace NorwegianBlue.Samples
{
    public class AnalysisNote
    {
        public enum AnalysisNoteType
        {
            Summary,
            Details
        }

        public enum AnalysisNotePriorities
        {
            Critical,
            Important,
            Informational,
            Debug
        }

        public string Name { get; private set; }
        public string Summary { get; private set; }
        public AnalysisNoteType NoteType { get; private set; }
        public AnalysisNotePriorities NotePriority { get; private set; }
        public List<PlotModel> GraphData { get; private set; }

        public AnalysisNote(string name, string summary, AnalysisNoteType noteType, AnalysisNotePriorities notePriority, List<PlotModel> graphData)
        {
            Name = name;
            Summary = summary;
            NoteType = noteType;
            NotePriority = notePriority;
            GraphData = graphData;
        }

    }
}
