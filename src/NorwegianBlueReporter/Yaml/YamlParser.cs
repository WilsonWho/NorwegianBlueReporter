using System.Collections.Generic;
using System.IO;
using System.Linq;
using NorwegianBlueReporter.Yaml.DTOs;
using YamlDotNet.Serialization;

namespace NorwegianBlueReporter.Yaml
{
    public class YamlParser
    {
        private static YamlParser _instance;

        public static YamlParser Instance
        {
            get { return _instance ?? (_instance = new YamlParser()); }
        }

        public T Deserialize<T>(string path)
        {
            var content = File.ReadAllText(path);

            var deserializer = new Deserializer();
            var configuration = deserializer.Deserialize<T>(new StringReader(content));

            return configuration;
        }

        public Rule GetConfiguration<T>(Configuration configuration)
        {
            return configuration.Rules.First(x => x.Type == typeof(T).Name);
        }
    }
}