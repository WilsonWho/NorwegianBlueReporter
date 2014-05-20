using System.Collections.Generic;
using NorwegianBlue.Samples;
using OxyPlot;

namespace NorwegianBlue.IagoIntegration.Analysis
{
    class IagoSampleSetAnalysisNote : AnalysisNote
    {
        public override string FriendlyTypeName
        {
            get { return @"Iago Sample Set Analysis"; }
        }

        public IagoSampleSetAnalysisNote(string name, string summary, AnalysisNoteDetailLevel noteDetailLevel,
                                AnalysisNotePriorities notePriority, List<PlotModel> graphData)
            : base(name, summary, noteDetailLevel, notePriority, graphData)
        {
        }
    }
}
