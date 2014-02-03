using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsReader
{
    public class AnalysisNote
    {
        public string name { get; private set; }
        public string summary { get; private set; }
        public Graph graph { get; private set; }

        AnalysisNote(string name, string summary, Graph graph)
        {
            this.name = name;
            this.summary = summary;
            this.graph = graph;
        }
    }
}
