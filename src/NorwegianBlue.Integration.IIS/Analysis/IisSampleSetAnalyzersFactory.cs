using System;
using System.Collections.Generic;
using NorwegianBlue.Analyzer;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Integration.IIS.Analysis
{
    public class IisSampleSetAnalyzersFactory : AbstractSampleSetAnalyzersFactory<IisSampleSetAnalyzers>
    {
        public override IisSampleSetAnalyzers Create()
        {
            var className = typeof (IisSampleSetAnalyzers).Name;
            var configuration = YamlParser.GetConfiguration(className);
            return Create(configuration);
        }

        public override IisSampleSetAnalyzers Create(Dictionary<object, object> configuration)
        {
            var instance = new IisSampleSetAnalyzers(configuration);
            return instance;
        }
    }
}
