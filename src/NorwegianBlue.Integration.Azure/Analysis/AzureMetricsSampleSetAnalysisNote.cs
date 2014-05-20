using System.Collections.Generic;
using NorwegianBlue.Samples;
using OxyPlot;

namespace NorwegianBlue.Integration.Azure.Analysis
{
    class AzureMetricsSampleSetAnalysisNote : AnalysisNote
    {
        public override string FriendlyTypeName
        {
            get { return @"Azure Sample Set Analysis"; }
        }

        public AzureMetricsSampleSetAnalysisNote(string name, string summary, AnalysisNoteDetailLevel noteDetailLevel,
                                AnalysisNotePriorities notePriority, List<PlotModel> graphData)
            : base(name, summary, noteDetailLevel, notePriority, graphData)
        {
        }

    }
}
