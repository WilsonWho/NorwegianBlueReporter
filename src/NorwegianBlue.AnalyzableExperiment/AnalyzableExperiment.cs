using System.Collections.Generic;
using NorwegianBlue.Data.Experiment;

namespace NorwegianBlue.AnalyzableExperiment
{
    public class AnalyzableExperiment : AbstractBaseExperimentWithAnalysis
    {
        internal AnalyzableExperiment(Dictionary<object, object> configuration) : base(configuration)
        {
        }
    }
}
