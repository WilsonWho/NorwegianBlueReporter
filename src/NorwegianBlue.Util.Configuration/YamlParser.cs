using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;

namespace NorwegianBlue.Util.Configuration
{
    public class YamlParser
    {
        private static dynamic _configuration;
        private static string _publicFileName;
        private static string _privateFileName;

        private static IDictionary<string, object> Configuration
        {
            get
            {
                return _configuration ?? (_configuration = DeserializeConfiguration());
            }
        }

        private static dynamic DeserializeConfiguration()
        {
            if (string.IsNullOrEmpty(_publicFileName))
            {
                _publicFileName = @"../../../config.yaml";
            }

            if (string.IsNullOrEmpty(_privateFileName))
            {
                _privateFileName = @"../../../private.yaml";
            }

            var deserializer = new Deserializer();

            var publicFileContent = File.ReadAllText(_publicFileName);
            var publicDeserializedContent = deserializer.Deserialize<ExpandoObject>(new StringReader(publicFileContent));
            
            var privateFileContent = File.ReadAllText(_privateFileName);
            var privateDeserializedContent = deserializer.Deserialize<ExpandoObject>(new StringReader(privateFileContent));

            return Merge(publicDeserializedContent, privateDeserializedContent);
        }

        public static void SetPublicConfigurationFile(string path)
        {
            _publicFileName = path;
        }

        public static void SetPrivateConfigurationFile(string path)
        {
            _privateFileName = path;
        }

        public static dynamic GetConfiguration()
        {
            var declaringType = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name;

            return Configuration[declaringType];
        }

        private static object Merge(IDictionary<string, object> obj1, IDictionary<string, object> obj2)
        {
            IDictionary<string, object> resultSet = new Dictionary<string, object>();

            foreach (var kvp in obj1)
            {
                resultSet.Add(kvp.Key, kvp.Value);
            }

            foreach (var kvp in obj2)
            {
                if (resultSet.ContainsKey(kvp.Key))
                {
                    var dict1 = ConvertDictionary((IDictionary<object, object>) obj1[kvp.Key]);
                    var dict2 = ConvertDictionary((IDictionary<object, object>) kvp.Value);

                    var mergedObj = Merge(dict1, dict2);
                    resultSet[kvp.Key] = mergedObj;
                }
                else
                {
                    resultSet.Add(kvp.Key, kvp.Value);
                }
            }

            return resultSet;
        }

        private static IDictionary<string, object> ConvertDictionary(IDictionary<object, object> obj)
        {
            IDictionary<string, object> convertedDictionary = obj.ToDictionary(x => x.Key.ToString(),
                                                                         x => x.Value);

            return convertedDictionary;
        }
    }
}