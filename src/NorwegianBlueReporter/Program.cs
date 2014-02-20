﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using LaserPrinter;
using LaserPrinter.Obsolete;
using MigraDoc.DocumentObjectModel;
using StatsReader;

namespace NorwegianBlueReporter
{
    class Program
    {
        static void Main(string[] args)
        {
            string markdown = string.Empty;

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

                if (!string.IsNullOrEmpty(options.AttachmentsDirectory))
                {
                    Console.WriteLine("Attachments taken from {0}", options.AttachmentsDirectory);
                }

                if (!string.IsNullOrEmpty(options.Markdown))
                {
                    Console.WriteLine("Using markdown file {0}", options.Markdown);

                    markdown = File.ReadAllText(options.Markdown);

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
            var statAnalyzers = new CommonStatAnalysis();
            var setAnalysisMethods = new List<SetAnalyzer>();
            var statAnalysisMethods = new List<StatAnalyzer>();

            // TODO: Populate these by reflection
            // Set analysis
            setAnalysisMethods.Add(setAnalyzers.FindAllHeaders);
            setAnalysisMethods.Add(setAnalyzers.SummaryStats);
            setAnalysisMethods.Add(setAnalyzers.ClusterAnalysis);

            // individual stat analysis
            statAnalysisMethods.Add(statAnalyzers.SummaryStatComparison);

            stats.Analyze(setAnalysisMethods, statAnalysisMethods);

            var document = new Document();
            var documentManager = new DocumentManager(document);

            // TODO: Immediately - read an intro chunk of markdown from a file and insert it. E.g. -intro= commandline option
            if (!string.IsNullOrEmpty(markdown))
            {
                documentManager.AddMarkdown(markdown);
            }

            // TODO: Immediately - fix the overly large headers
            var headerStyle = new Style("MdHeading1", "Normal")
                {
                    Font =
                        {
                            Size = "10pt"
                        }
                };

            document.Add(headerStyle);
            documentManager.AddMarkdown(@"# All Stats
Some text.

Some more text...
");

            // Add the Analysis Notes for the set
            foreach (var analysisNote in stats.AnalysisNotes)
            {
                documentManager.AppendAnalysisNote(analysisNote);
            }

            documentManager.AddMarkdown(@"# Each Stat");

            foreach (var stat in stats.Statistics)
            {
                var timestamp = string.Format("###{0}\n", stat.TimeStamp);
                documentManager.AppendMarkdown(timestamp);
                foreach (var analysisNote in stat.AnalysisNotes)
                {
                    documentManager.AppendAnalysisNote(analysisNote);
                }
            }

            const string fileName = "Experiment Alpha.pdf";
            const string ext = ".pdf";
            documentManager.SaveAsPdf(fileName);

            Process.Start(fileName);
        }
    }
}
