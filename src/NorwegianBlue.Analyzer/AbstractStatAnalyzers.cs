using System.Collections.Generic;

namespace NorwegianBlue.Analyzer
{
    abstract public class AbstractStatAnalyzers : AbstractAnalyzers
    {
        protected AbstractStatAnalyzers(Dictionary<object, object> configuration) : base(configuration)
        {
        }
    }
}