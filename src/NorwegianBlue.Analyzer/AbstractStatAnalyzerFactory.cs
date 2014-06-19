using System.Collections.Generic;

namespace NorwegianBlue.Analyzer
{
    abstract public class AbstractStatAnalyzerFactory<T> where T: AbstractStatAnalyzers
    {
        public abstract T Create();
        public abstract T Create(Dictionary<object, object> configuration);
    }
}
