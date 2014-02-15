using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using LaserPrinter;
using LaserPrinter.Graphs;
using LaserPrinter.Obsolete;
using MigraDoc.DocumentObjectModel;
using StatsReader;
using Graph = LaserPrinter.Graphs.Graph;
using GraphType = LaserPrinter.Obsolete.GraphType;

namespace NorwegianBlueReporter
{
    class Program
    {
        static void Main(string[] args)
        {
            const string filename = @"C:\dev\NorwegianBlueReporter\src\StatsReader\parrot-server-stats.log";
            StreamReader reader = File.OpenText(filename);
            var stats = new IagoStatisticsSet();
            stats.Parse(reader);

            var setAnalyzers = new CommonStatSetAnalysis();
            var setAnalysisMethods = new List<SetAnalyzer>();
            var statAnalysisMethods = new List<StatAnalyzer>();

            setAnalysisMethods.Add(setAnalyzers.FindAllHeaders);
            setAnalysisMethods.Add(setAnalyzers.SummaryStats);

            stats.Analyze(setAnalysisMethods, statAnalysisMethods);

            var analysisNote = stats.AnalysisNotes[0];


            var document = new Document();
            var documentManager = new DocumentManager(document);
            //documentManager.AddMarkDown(analysisNote.Summary);
            //documentManager.CreateGraphSection(GraphType.ColumnStacked, analysisNote.Graph.SeriesData);

            //var graph = new ColumnStackedGraph("Column Stacked Graph", false, Graph.LegendPositionEnum.Left, false, analysisNote.GraphData.SeriesData);
            //graph.Draw(document);

            const string fileName = "Experiment Alpha";
            documentManager.SaveAsPdf(fileName);

            Process.Start(fileName);
        }
    }
}
