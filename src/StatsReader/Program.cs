using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = @"C:\dev\NorwegianBlueReporter\src\StatsReader\parrot-server-stats.log";
            StreamReader reader = File.OpenText(filename);
            var stats = new IagoStatisticsSet();
            stats.Parse(reader);

            Console.WriteLine("-----------------------");
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();

        }
    }
}
