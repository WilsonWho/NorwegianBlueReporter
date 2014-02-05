using System;
using System.Collections.Generic;
using System.IO;

namespace StatsReader
{
    class Program
    {
        static void Main()
        {
            const string filename = @"C:\dev\NorwegianBlueReporter\src\StatsReader\parrot-server-stats.log";
            StreamReader reader = File.OpenText(filename);
            var stats = new IagoStatisticsSet();
            stats.Parse(reader);

            var setAnalyzers = new CommonStatSetAnalysis();
            var setAnalysisMethods = new List<SetAnalyzer>();
            var statAnalysisMethods = new List<StatAnalyzer>();
 
            setAnalysisMethods.Add(setAnalyzers.FindAllHeaders);
            setAnalysisMethods.Add(setAnalyzers.SummaryStats);

            stats.Analyze(setAnalysisMethods, statAnalysisMethods);

            Console.WriteLine("-----------------------");
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();

        }
    }
}
