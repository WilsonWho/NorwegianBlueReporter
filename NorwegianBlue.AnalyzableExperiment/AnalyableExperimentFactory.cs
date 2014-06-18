using System.Collections.Generic;
using NorwegianBlue.Data.Experiment;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.AnalyzableExperiment
{
    class AnalyableExperimentFactory : AbstractExperimentWithAnalysisFactory<AnalyzableExperiment>
    {
        public override AnalyzableExperiment Create()
        {
            var className = typeof(AnalyzableExperiment).Name;
            var configuration = YamlParser.GetConfiguration(className);
            return Create(configuration);
        }

        public override AnalyzableExperiment Create(Dictionary<object, object> configuration)
        {
            var instance = new AnalyzableExperiment(configuration);
            return instance;
        }
    }
}
