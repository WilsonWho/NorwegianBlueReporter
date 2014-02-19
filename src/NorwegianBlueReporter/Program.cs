using System;
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
using FSharp.Markdown.Pdf;

namespace NorwegianBlueReporter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parse command line arguments
            var options = new CommandLineOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (!string.IsNullOrEmpty(options.InputFileName))
                {
                    Console.WriteLine("Input file: {0}", options.InputFileName);
                }

                if (!string.IsNullOrEmpty(options.OutputFileName))
                {
                    Console.WriteLine("File saved as {0}", options.OutputFileName);
                }
            }
            else
            {
                throw new ArgumentException("Invalid command line arguments!");
            }

            const string filename = @"C:\tmp\parrot-server-stats.log";
            StreamReader reader = File.OpenText(filename);
            var stats = new IagoStatisticsSet();
            stats.Parse(reader);

            var setAnalyzers = new CommonStatSetAnalysis();
            var setAnalysisMethods = new List<SetAnalyzer>();
            var statAnalysisMethods = new List<StatAnalyzer>();

            setAnalysisMethods.Add(setAnalyzers.FindAllHeaders);
            setAnalysisMethods.Add(setAnalyzers.SummaryStats);
            setAnalysisMethods.Add(setAnalyzers.ClusterAnalysis);

            stats.Analyze(setAnalysisMethods, statAnalysisMethods);

            var analysisNote = stats.AnalysisNotes[0];


            var document = new Document();
            var documentManager = new DocumentManager(document);
            //documentManager.AddMarkDown(analysisNote.Summary);

            document.AddSection();

            MarkdownPdf.AddMarkdown(document, document.LastSection, analysisNote.Summary);

            var ctg = new ColorTableGraph(analysisNote.GraphData);
            ctg.Draw(document);

            const string fileName = "Experiment Alpha";
            documentManager.SaveAsPdf(fileName);

            Process.Start(fileName);
        }
    }
}
