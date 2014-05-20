using System.Collections.Generic;
using NorwegianBlue.Samples;
using OxyPlot;

namespace NorwegianBlue.Integration.IIS.Analysis
{
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
