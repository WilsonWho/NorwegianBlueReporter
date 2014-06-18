using System.Collections.Generic;
using NorwegianBlue.Notes.AnalysisNotes;
using NorwegianBlue.Reporting;
using OxyPlot;

namespace NorwegianBlue.Integration.IIS.Analysis
{
    [ReportingMetaData(ReportingTypes.SampleSet)]
    class IisSampleSetAnalysisNote:AnalysisNote
    {
        public override string FriendlyTypeName
        {
            get { return @"IIS Analysis"; }
        }

        public IisSampleSetAnalysisNote(string name, string summary, AnalysisNoteDetailLevel noteDetailLevel,
                                AnalysisNotePriorities notePriority, List<PlotModel> graphData)
            : base(name, summary, noteDetailLevel, notePriority, graphData)
        {
        }
    }
}
