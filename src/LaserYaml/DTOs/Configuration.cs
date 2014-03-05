using System.Collections.Generic;

namespace LaserYaml.DTOs
{
    public class Configuration
    {
        public AppOptions AppOptions { get; set; }
        public List<Rule> Rules { get; set; }
    }
}