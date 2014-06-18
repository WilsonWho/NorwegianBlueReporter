using System.Collections.Generic;
using NorwegianBlue.Data.Sample;

namespace NorwegianBlue.Data.SampleSet
{
    abstract public class AbstractSampleSetWithAnalysisFactory<T1, T2> where T1: ISampleSetAnalysis<T2> where T2: ISampleAnalysis
    {
        public abstract T1 Create();
        public abstract T1 Create(Dictionary<object, object> configuration);
    }
}
