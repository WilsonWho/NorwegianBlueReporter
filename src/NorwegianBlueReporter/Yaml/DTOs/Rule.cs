using System.Collections.Generic;

namespace NorwegianBlueReporter.Yaml.DTOs
{
    public class Rule
    {
        public string Type { get; set; }
        public List<string> Filters { get; set; }
        public Settings Settings { get; set; }
    }
}