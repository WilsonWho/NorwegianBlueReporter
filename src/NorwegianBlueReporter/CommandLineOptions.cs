using CommandLine;
using CommandLine.Text;

namespace NorwegianBlueReporter
{
    public class CommandLineOptions
    {
        [Option('i', "input", Required = true, HelpText = "Input file name")]
        public string InputFileName { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output file name")]
        public string OutputFileName { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}