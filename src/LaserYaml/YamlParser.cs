using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using YamlDotNet.Serialization;

namespace LaserYaml
{
    public class YamlParser
    {
        private static dynamic _configuration;
        private static string _fileName;

        private static ExpandoObject Configuration
        {
            get
            {
                return _configuration ?? (_configuration = DeserializeConfiguration());
            }
        }

        private static dynamic DeserializeConfiguration()
        {
            if (string.IsNullOrEmpty(_fileName))
            {
                _fileName = @"../../../config.yaml";
            }

            var content = File.ReadAllText(_fileName);
            var deserializer = new Deserializer();

            return deserializer.Deserialize<ExpandoObject>(new StringReader(content));
        }

        public static void SetConfigurationFile(string path)
        {
            _fileName = path;
        }

        public static dynamic GetConfiguration()
        {
            var declaringType = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name;
            var configuration = (IDictionary<string, object>)Configuration;

            return configuration[declaringType];
        }
    }
}