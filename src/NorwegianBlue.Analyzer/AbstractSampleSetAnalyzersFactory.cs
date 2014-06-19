using System.Collections.Generic;

namespace NorwegianBlue.Analyzer
{
    abstract public class AbstractSampleSetAnalyzersFactory<T> where T: AbstractSampleSetAnalyzers
    {
        public abstract T Create();
        public abstract T Create(Dictionary<object, object> configuration);
    }
}
