using System.Collections.Generic;
using NorwegianBlue.Analyzer;

namespace NorwegianBlue.CommonAnalyzers.Algorithms
{
    public class CommonStatAnalyzersFactory : AbstractStatAnalyzersFactory<CommonStatAnalyzers>
    {
        public override CommonStatAnalyzers Create()
        {
            var configuration = new Dictionary<object, object>();
            return Create(configuration);
        }

        public override CommonStatAnalyzers Create(Dictionary<object, object> configuration)
        {
            var instance = new CommonStatAnalyzers(configuration);
            return instance;
        }
    }
}
