using System;
using System.Collections.Generic;
using System.Reflection;
using NorwegianBlue.Analyzer;
using NorwegianBlue.Data.Experiment;
using NorwegianBlue.Data.SampleSet;
using NorwegianBlue.Util.Common;
using NorwegianBlue.Util.Configuration;

namespace NorwegianBlue.Reporter
{
    public class Reporter
    {
        private dynamic Configuration { get; set; }

        private readonly List<Type> experimentWithAnalysisFactories = new List<Type>();
        private readonly List<Type> sampleSetWithAnalysisFactories = new List<Type>();
        private readonly List<Type> statAnalyzersFactories = new List<Type>();
        private readonly List<Type> sampleSetAnalyzersFactories = new List<Type>();

        // rather than a number of 'if' statements, set up the things to match in a data structure
        // and apply a simple rule to each item.
        private readonly List<Tuple<Type, List<Type>>> factoriesToMatch = new List<Tuple<Type, List<Type>>>();

        public Reporter(dynamic configuration)
        {
            Configuration = configuration;
            factoriesToMatch.Add(new Tuple<Type, List<Type>>(typeof (AbstractExperimentWithAnalysisFactory<>), experimentWithAnalysisFactories));
            factoriesToMatch.Add(new Tuple<Type, List<Type>>(typeof (AbstractSampleSetWithAnalysisFactory<,>), sampleSetWithAnalysisFactories));
            factoriesToMatch.Add(new Tuple<Type, List<Type>>(typeof (AbstractStatAnalyzersFactory<>), statAnalyzersFactories));
            factoriesToMatch.Add(new Tuple<Type, List<Type>>(typeof (AbstractSampleSetAnalyzersFactory<>), sampleSetAnalyzersFactories));

        }

        public void DoWork()
        {
            
        }



        private void FindFactories()
        {
            var types = new List<Type>();

            List<object> dataProcessingAssemblyNames = Configuration["DataProcessingAssemblies"];

            foreach (var assemblyName in dataProcessingAssemblyNames)
            {
                Assembly assembly = Assembly.Load(assemblyName.ToString());
                foreach (Type type in assembly.GetTypes())
                {
                    types.Add(type);
                }
            }


            foreach (var type in types)
            {
                foreach (var factoryMatch in factoriesToMatch)
                {
                    var genericFactoryType = factoryMatch.Item1;
                    var concreteFactoryList = factoryMatch.Item2;
                    if (type.IsSubclassOfGeneric(genericFactoryType) && !type.IsAbstract)
                    {
                        concreteFactoryList.Add(type);
                    }
                }
            }
        




        }

    }
}
