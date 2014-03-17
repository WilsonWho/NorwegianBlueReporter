using System;
using System.Collections.Generic;
using System.IO;
using LaserOptics.Common;
using LaserOptics.IagoStats;

namespace LaserOptics
{
    class Program
    {
        static void Main()
        {
            const string filename = @"C:\git\NorwegianBlueReporter\src\StatsReader\parrot-server-stats.log";
            StreamReader reader = File.OpenText(filename);
            var stats = new IagoSampleSet();
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
