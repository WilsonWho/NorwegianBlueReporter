using System.Collections.Generic;

namespace NorwegianBlue.Analyzer
{
    abstract public class AbstractSampleSetAnalyzers : AbstractAnalyzers
    {
        protected AbstractSampleSetAnalyzers(Dictionary<object, object> configuration) : base(configuration)
        {
        }
    }
}