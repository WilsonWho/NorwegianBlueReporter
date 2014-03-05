using System.Collections.Generic;

namespace LaserYaml.DTOs
{
    public class AppOptions
    {
        public string OutputFileName { get; set; }
        public string AttachmentsDirectory { get; set; }
        public string MarkdownNotesFileName { get; set; }
        public Dictionary<string, string> InputFileNames { get; set; }
        public string GraphType { get; set; }
    }
}
