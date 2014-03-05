using System;
using System.Collections.Generic;
using System.IO;
using LaserOptics.Common;
using LaserOptics.IagoStats;
using LaserYaml.DTOs;

namespace LaserOptics
{
    class Program
    {
        static void Main()
        {
            const string filename = @"C:\git\NorwegianBlueReporter\src\StatsReader\parrot-server-stats.log";
            StreamReader reader = File.OpenText(filename);
            var stats = new IagoStatisticsSet(new Configuration()); // Creating an empty configuration just to make this work
            stats.Parse(reader);

            var setAnalyzers = new CommonStatSetAnalysis();
            var statAnalyzers = new CommonStatAnalysis();

            var setAnalysisMethods = new List<SetAnalyzer>();
            var statAnalysisMethods = new List<StatAnalyzer>();
 
            setAnalysisMethods.Add(setAnalyzers.FindAllHeaders);
            setAnalysisMethods.Add(setAnalyzers.SummaryStats);
            setAnalysisMethods.Add(setAnalyzers.ClusterAnalysis);

            statAnalysisMethods.Add(statAnalyzers.SummaryStatComparison);

            stats.Analyze(setAnalysisMethods, statAnalysisMethods);

            Console.WriteLine("-----------------------");
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();

        }
    }
}
