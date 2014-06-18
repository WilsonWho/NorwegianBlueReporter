using System.Collections.Generic;
using NorwegianBlue.Data.Sample;
using NorwegianBlue.Data.SampleSet;

namespace NorwegianBlue.Data.Experiment
{
    abstract public class AbstractExperimentWithAnalysisFactory<T> where T: IExperiment<ISampleSetValues<ISampleValues>>, IExperimentAnalysis<ISampleAnalysis> 
    {
        public abstract T Create();
        public abstract T Create(Dictionary<object, object> configuration);
    }
}
