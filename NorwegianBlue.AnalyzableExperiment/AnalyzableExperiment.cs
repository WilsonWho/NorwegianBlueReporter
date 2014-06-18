using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NorwegianBlue.Data.Experiment;

namespace NorwegianBlue.AnalyzableExperiment
{
    public class AnalyzableExperiment : BaseExperimentWithAnalysis
    {
        internal AnalyzableExperiment(Dictionary<object, object> configuration) : base(configuration)
        {
        }
    }
}
