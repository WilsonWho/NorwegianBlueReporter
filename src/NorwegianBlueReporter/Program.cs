using System;
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
            string fileName = string.Empty;
            string output = string.Empty;

            // Parse command line arguments
            var options = new CommandLineOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (!string.IsNullOrEmpty(options.InputFileName))
                {
                    Console.WriteLine("Input file: {0}", options.InputFileName);
                    fileName = options.InputFileName;
                }
                else
                {
                    throw new ArgumentException("No stats log was provided ...");
                }

                if (!string.IsNullOrEmpty(options.OutputFileName))
                {
                    Console.WriteLine("File saved as {0}", options.OutputFileName);
                }
                else
                {
                    throw new ArgumentException("No output file was specified ...");
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

            StreamReader reader = File.OpenText(fileName);
            var stats = new IagoStatisticsSet();
            stats.Parse(reader);

            var setAnalyzers = new CommonStatSetAnalysis();
            var statAnalyzers = new CommonStatAnalysis();
            var setAnalysisMethods = new List<SetAnalyzer>();
            var statAnalysisMethods = new List<StatAnalyzer>();

            // TODO: Populate these by reflection
            // Set analysis
            setAnalysisMethods.Add(setAnalyzers.FindAllHeaders);
            setAnalysisMethods.Add(setAnalyzers.ClusterAnalysis);
            setAnalysisMethods.Add(setAnalyzers.SummaryStats);

            // individual stat analysis
            // statAnalysisMethods.Add(statAnalyzers.SummaryStatComparison);
            statAnalysisMethods.Add(statAnalyzers.SummaryStatComparisonAsTables);

            stats.Analyze(setAnalysisMethods, statAnalysisMethods);

            //------------------------------------------------------------------

            var document = new Document();
            
            // Add notes about the previous report if available
            if (!string.IsNullOrEmpty(markdown))
            {
                document.AddMarkdown(markdown);
            }

            // TODO: Immediately - read an intro chunk of markdown from a file and insert it. E.g. -intro= commandline option
            if (!string.IsNullOrEmpty(markdown))
            {
                document.AddMarkdown(markdown);
            }

            // TODO: Immediately - fix the overly large headers
            const int numHeaderLevels = 6;
            for (int i = 1; i <= numHeaderLevels; i++)
            {
                var mdHeaderName = string.Format("MdHeading{0}", i);
                var mdHeaderBaseName = string.Format("Heading{0}", i);
                var mdHeaderStyle = new Style(mdHeaderName, mdHeaderBaseName);
                mdHeaderStyle.Font.Bold = true;
                mdHeaderStyle.Font.Size = 8 + (numHeaderLevels - i)*2;
                mdHeaderStyle.ParagraphFormat.SpaceBefore = Unit.FromPoint(numHeaderLevels - i);
                document.Add(mdHeaderStyle);
            }

            var style = document.Styles.Normal;
            style.ParagraphFormat.SpaceBefore = Unit.FromPoint(6d);
            style.ParagraphFormat.SpaceAfter = Unit.FromPoint(6d);
 
            document.AddMarkdown(@"# All Stats
Some text.

Some more text...
");

            // Add the Analysis Notes for the set
            foreach (var analysisNote in stats.AnalysisNotes)
            {
                document.AppendAnalysisNote(analysisNote);
            }

            document.AddMarkdown(@"# Each Stat");

            foreach (var stat in stats.Statistics)
            {
                var timestamp = string.Format("###{0}\n", stat.TimeStamp);
                document.AppendMarkdown(timestamp);
                foreach (var analysisNote in stat.AnalysisNotes)
                {
                    document.AppendAnalysisNote(analysisNote);
                }
            }


            //TODO: would be nice to reduce the size of the "Each Stats" output section
            // below worked for all top level paragraphs, but needs to recursively go through each
            // element.Elements recursively and apply the font size to each paragraph.
            // NOTE: using GetType/ "is" from object worked fine for me, but making element a 
            // DocumentElement -> I couldn't use it as a paragraph etc...???? Odd???
            //foreach (object element in document.LastSection.Elements)
            //{                
            //    if (element is Paragraph)
            //    {
            //        (element as Paragraph).Format.Font.Size = Unit.FromPoint(8);
            //    }
            //}

            const string ext = ".pdf";
            document.SaveFile(output, ext);

            //Process.Start(fileName);


            Console.WriteLine("-----------------------");
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
        }
    }
}
