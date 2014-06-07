using System.Collections.Generic;
using NorwegianBlue.Analysis;
using NorwegianBlue.Samples;
using OxyPlot;

namespace NorwegianBlue.Integration.Azure.Analysis
{
    class AzureMetricsSampleAnalysisNote : AnalysisNote
    {
        public override string FriendlyTypeName
        {
            get { return @"Azure Analysis"; }
        }

        public AzureMetricsSampleAnalysisNote(string name, string summary, AnalysisNoteDetailLevel noteDetailLevel,
                                AnalysisNotePriorities notePriority, List<PlotModel> graphData)
            : base(name, summary, noteDetailLevel, notePriority, graphData)
        {
        }

    }
}
