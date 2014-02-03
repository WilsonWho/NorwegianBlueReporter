using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace StatsReader
{
    public static class Utils
    {

    /// <summary>
    /// Source: http://stackoverflow.com/questions/78536/deep-cloning-objects-in-c-sharp
    /// Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
    /// Provides a method for performing a deep copy of an object.
    /// Binary Serialization is used to perform the copy.
    /// </summary>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        public static ReadOnlyDictionary<TKey, TValue> ReadOnlyDictionaryWithCopiedValues<TKey, TValue>(Dictionary<TKey, TValue> input)
        {
            var copyDict = new Dictionary<TKey, TValue>();
            foreach (var kvp in input)
            {
                copyDict[kvp.Key] = kvp.Value.Clone();
            }
            return new ReadOnlyDictionary<TKey, TValue>(copyDict);
        }

        public static ReadOnlyCollection<TValue> ReadOnlyCollectionWithCopiedValues<TValue>(IEnumerable<TValue> input)
        {
            var copyList = input.Select(val => val.Clone()).ToList();
            return new ReadOnlyCollection<TValue>(copyList);
        }



    }
}
