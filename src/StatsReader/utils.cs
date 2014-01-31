using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsReader
{
    class Utils
    {
        public static ReadOnlyDictionary<TKey, TValue> ReadOnlyDictionaryWithCopiedValues<TKey, TValue>(Dictionary<TKey, TValue> input)
        {
            var copyDict = new Dictionary<TKey, TValue>();
            foreach (var kvp in input)
            {
                copyDict[kvp.Key] = kvp.Value.Copy();
            }
            return new ReadOnlyDictionary<TKey, TValue>(copyDict);
        }

        public static ReadOnlyCollection<TValue> ReadOnlyCollectionWithCopiedValues<TValue>(IEnumerable<TValue> input)
        {
            var copyList = new List<TValue>();
            foreach (var val in input)
            {
                copyList.Add(val.Copy());
            }
            return new ReadOnlyCollection<TValue>(copyList);
        }



    }
}
