using System;
using System.Collections.Generic;

namespace NorwegianBlue.Data.Sample
{
    abstract public class AbstractSampleWithAnalysisFactory<T> where T: ISampleAnalysis
    {
        public abstract T Create(DateTime timeStamp, IEnumerable<Tuple<string, string>> data);
        public abstract T Create(DateTime timeStamp, IEnumerable<Tuple<string, string>> data, Dictionary<object, object> configuration);
    }
}
