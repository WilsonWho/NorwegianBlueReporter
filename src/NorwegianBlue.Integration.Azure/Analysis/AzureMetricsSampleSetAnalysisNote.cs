using System.Collections.Generic;
using NorwegianBlue.Analysis;
using NorwegianBlue.Reporting;
using OxyPlot;

namespace NorwegianBlue.Integration.Azure.Analysis
{
    [ReportingMetaData(ReportingTypes.SampleSet)]
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
