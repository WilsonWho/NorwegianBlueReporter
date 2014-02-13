using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using LaserPrinter;
using MigraDoc.DocumentObjectModel;
using StatsReader;
using GraphType = LaserPrinter.GraphType;

namespace NorwegianBlueReporter
{
    class Program
    {
        static void Main(string[] args)
        {
            const string filename = @"C:\git\NorwegianBlueReporter\src\StatsReader\parrot-server-stats.log";
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
            documentManager.AddMarkDown(analysisNote.Summary);

            
            documentManager.CreateGraphSection(GraphType.ColumnStacked, analysisNote.Graph.SeriesData);


            const string fileName = "Experiment Alpha";
            documentManager.SaveAsPdf(fileName);

            Process.Start(fileName);
        }
    }
}
