using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using LaserPrinter;
using MigraDoc.DocumentObjectModel;
using LaserOptics;

namespace NorwegianBlueReporter
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = ParseCommandLineArgs(args);

            StreamReader reader = File.OpenText(options.InputFileNames[typeof(IagoStatisticsSet)]);
            var stats = new IagoStatisticsSet();
            stats.Parse(reader);

            reader.Close();

            var setAnalyzers = new CommonStatSetAnalysis();
            var statAnalyzers = new CommonStatAnalysis();
            var iagoSetAnalyzers = new IagoStatSetAnalysis();

            var setAnalysisMethods = new List<SetAnalyzer>();
            var statAnalysisMethods = new List<StatAnalyzer>();

            // TODO: Populate these by reflection
            // Set analysis
            setAnalysisMethods.Add(setAnalyzers.FindAllHeaders);
            setAnalysisMethods.Add(setAnalyzers.ClusterAnalysis);
            setAnalysisMethods.Add(setAnalyzers.SummaryStats);
            setAnalysisMethods.Add(iagoSetAnalyzers.IagoRequestLatencySummary);

            // individual stat analysis
            // statAnalysisMethods.Add(statAnalyzers.SummaryStatComparison);
            statAnalysisMethods.Add(statAnalyzers.SummaryStatComparisonAsTables);

            stats.Analyze(setAnalysisMethods, statAnalysisMethods);

            //------------------------------------------------------------------

            var document = new Document();
            
            // Add notes about the previous report if available
            if (!string.IsNullOrEmpty(options.MarkdownNotesFileName))
            {
                var markdown = File.ReadAllText(options.MarkdownNotesFileName);
                document.AddMarkdown(markdown);
            }

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
 
            document.AddMarkdown(@"# Analysis of the Aggregate Set

The following sections are various analysis over the entire set of data collected.
");

            // Add the Analysis Notes for the set
            foreach (var analysisNote in stats.AnalysisNotes)
            {
                document.AppendAnalysisNote(analysisNote);
            }

            document.AddMarkdown(@"# Each Stat

The following sections are an analysis for anomolies for each entry in the collected stats file.

");

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
            document.SaveFile(options.OutputFileName, ext);

            //Process.Start(fileName);

            // pause if a debugger is running, so we can see any console output
            if (Debugger.IsAttached)
            {
                Console.WriteLine("-----------------------");
                Console.WriteLine("Press enter to close...");
                Console.ReadLine();
            }
        }

        static AppOptions ParseCommandLineArgs(string[] args)
        {
            string iagoStatsInputFileName = null;
            string outputFileName = null;
            string attachmentsSourceDirectory = null;
            string markdownFileName = null;


            // Parse command line arguments
            var options = new CommandLineOptions();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (!string.IsNullOrEmpty(options.InputFileName))
                {
                    Console.WriteLine("Input file: {0}", options.InputFileName);
                    iagoStatsInputFileName = options.InputFileName;
                }
                else
                {
                    throw new ArgumentException("No stats log was provided ...");
                }

                if (!string.IsNullOrEmpty(options.OutputFileName))
                {
                    Console.WriteLine("File saved as {0}", options.OutputFileName);
                    outputFileName = options.OutputFileName;
                }
                else
                {
                    throw new ArgumentException("No output file was specified ...");
                }

                if (!string.IsNullOrEmpty(options.AttachmentsDirectory))
                {
                    Console.WriteLine("Attachments taken from {0}", options.AttachmentsDirectory);
                    attachmentsSourceDirectory = options.AttachmentsDirectory;
                }

                if (!string.IsNullOrEmpty(options.Markdown))
                {
                    Console.WriteLine("Using markdown file {0}", options.Markdown);

                    markdownFileName = options.Markdown;
                }
            }
            else
            {
                throw new ArgumentException("Invalid command line arguments!");
            }


            return new AppOptions(new Dictionary<Type, string>() {{typeof(IagoStatisticsSet), iagoStatsInputFileName}},
                                  outputFileName, attachmentsSourceDirectory, markdownFileName);
        }
    }
}
