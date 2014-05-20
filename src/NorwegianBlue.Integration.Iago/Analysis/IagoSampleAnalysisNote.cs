using System.Collections.Generic;
using NorwegianBlue.Samples;
using OxyPlot;

namespace NorwegianBlue.IagoIntegration.Analysis
{
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
