using System.Collections.Generic;
using NorwegianBlue.Analyzer;

namespace NorwegianBlue.CommonAnalyzers.Algorithms
{
    public class CommonSampleSetAnalyzersFactory : AbstractSampleSetAnalyzersFactory<CommonSampleSetAnalyzers>
    {
        public override CommonSampleSetAnalyzers Create()
        {
            var configuration = new Dictionary<object, object>();
            return Create(configuration);
        }

        public override CommonSampleSetAnalyzers Create(Dictionary<object, object> configuration)
        {
            var instance = new CommonSampleSetAnalyzers(configuration);
            return instance;
        }
    }
}
