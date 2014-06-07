using System;
using System.Collections.Generic;
using System.Text;
using OxyPlot;

namespace NorwegianBlue.Analysis
{
    public enum AnalysisNoteDetailLevel
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
    
    public abstract class AnalysisNote
    {
        public abstract string FriendlyTypeName { get; }
        public string Title { get; private set; }
        public string Summary { get; private set; }
        public AnalysisNoteDetailLevel NoteDetailLevel { get; private set; }
        public AnalysisNotePriorities NotePriority { get; private set; }
        public List<PlotModel> GraphData { get; private set; }

        protected AnalysisNote( string name, string summary, AnalysisNoteDetailLevel noteDetailLevel, 
                                AnalysisNotePriorities notePriority, List<PlotModel> graphData)
        {
            Title = name;
            Summary = summary;
            NoteDetailLevel = noteDetailLevel;
            NotePriority = notePriority;
            GraphData = graphData;
        }

        public override string ToString()
        {
            var output = new StringBuilder();
            output.AppendFormat("Title: {0}", Title);
            output.AppendLine();
            output.AppendFormat("Friendly Type Name: {0}", FriendlyTypeName);
            output.AppendLine();
            output.AppendFormat("Type name: {0}", GetType().Name);
            output.AppendFormat("Summary: {0}", Summary);
            output.AppendLine();
            output.AppendFormat("Note Detail Level: {0}", Enum.GetName(typeof(AnalysisNoteDetailLevel), NoteDetailLevel));
            output.AppendLine();
            output.AppendFormat("Note Priority: {0}", Enum.GetName(typeof(AnalysisNotePriorities), NotePriority));
            output.AppendLine();
            output.AppendFormat("Number of Graphs in this note: {0}", GraphData.Count);
            output.AppendLine();
            return output.ToString();
        }
    }
}
