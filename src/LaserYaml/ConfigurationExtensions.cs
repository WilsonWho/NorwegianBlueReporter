using System.Linq;
using LaserYaml.DTOs;

namespace LaserYaml
{
    public static class ConfigurationExtensions
    {
         public static Rule GetConfigurationFor<T>(this Configuration configuration)
         {
             return configuration.Rules.First(x => x.Type == typeof (T).Name);
         }
    }
}