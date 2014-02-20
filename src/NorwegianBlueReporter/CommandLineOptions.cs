using CommandLine;
using CommandLine.Text;

namespace NorwegianBlueReporter
{
    public class CommandLineOptions
    {
        [Option('i', "input", Required = true, HelpText = "Full path to the input file")]
        public string InputFileName { get; set; }

        [Option('o', "output", Required = true, HelpText = "Path and output name of the document that is generated")]
        public string OutputFileName { get; set; }

        [Option('a', "attachments", Required = false, HelpText = "Path of directory containing all attachments to be added to the PDF")]
        public string AttachmentsDirectory { get; set; }

        [Option('m', "markdown", Required = false, HelpText = "Directory and file name for the markdown file")]
        public string Markdown { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}