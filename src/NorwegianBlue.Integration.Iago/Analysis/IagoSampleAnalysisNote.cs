using System.Collections.Generic;
using NorwegianBlue.Notes.AnalysisNotes;
using NorwegianBlue.Reporting;
using OxyPlot;

namespace NorwegianBlue.Integration.Iago.Analysis
{
    [ReportingMetaData(ReportingTypes.Sample)]
    class IagoSampleAnalysisNote : AnalysisNote
    {
        public override string FriendlyTypeName
        {
            get { return @"Iago Analysis"; }
        }

        public IagoSampleAnalysisNote(string name, string summary, AnalysisNoteDetailLevel noteDetailLevel,
                                AnalysisNotePriorities notePriority, List<PlotModel> graphData)
            : base(name, summary, noteDetailLevel, notePriority, graphData)
        {
        }

    }
}
