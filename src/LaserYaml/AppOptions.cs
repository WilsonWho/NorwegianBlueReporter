using System.Collections.Generic;

namespace LaserYaml
{
    public class AppOptions
    {
        private readonly dynamic _configuration;

        public string OutputFileName { get; set; }
        public string AttachmentsDirectory { get; set; }
        public string MarkdownNotesFileName { get; set; }
        public Dictionary<string, string> InputFileNames { get; set; }
        public string GraphType { get; set; }

        public AppOptions(string outputFileName, string attachmentsDirectory, string markdownNotesFileName, Dictionary<string, string> inputFileNames, string graphType)
        {
            _configuration = YamlParser.GetConfiguration();

            OutputFileName = outputFileName ?? (string)_configuration["OutputFileName"];
            AttachmentsDirectory = attachmentsDirectory ?? (string)_configuration["AttachmentsDirectory"];
            MarkdownNotesFileName = markdownNotesFileName ?? (string)_configuration["MarkdownNotesFileName"];
            InputFileNames = inputFileNames ?? (Dictionary<string, string>)_configuration["InputFileNames"];
            GraphType = graphType ?? (string)_configuration["GraphType"];
        }
    }
}
