using System.IO;
using YamlDotNet.Serialization;

namespace LaserYaml
{
    public class YamlParser
    {
        private static YamlParser _instance;

        public static YamlParser Instance
        {
            get { return _instance ?? (_instance = new YamlParser()); }
        }

        public T Deserialize<T>(string content)
        {
            var deserializer = new Deserializer();
            var configuration = deserializer.Deserialize<T>(new StringReader(content));

            return configuration;
        }
    }
}