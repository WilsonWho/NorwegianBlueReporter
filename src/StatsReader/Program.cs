using System;
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

            stats.Analyze();

            Console.WriteLine("-----------------------");
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();

        }
    }
}
