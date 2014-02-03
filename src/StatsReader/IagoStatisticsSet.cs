using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsReader
{
    internal class IagoStatisticsSet : IStatisticsSet, IStatisticsSetSelfAnalysis, IStatisticsSetValues
    {
        private readonly dynamic _analysisScratchPad = new ExpandoObject();
        private readonly List<IagoStatistics> _iagoStatistics = new List<IagoStatistics>();
        private readonly List<AnalysisNote> _analysisNotes = new List<AnalysisNote>(); 


        ReadOnlyCollection<IStatisticsValues> IStatisticsSetValues.Statistics
        {
            get { return Utils.ReadOnlyCollectionWithCopiedValues(_iagoStatistics.Cast<IStatisticsValues>()); }
        }

        ReadOnlyCollection<AnalysisNote> IStatisticsSetValues.AnalysisNotes
        {
            get { return Utils.ReadOnlyCollectionWithCopiedValues(_analysisNotes); }
        }

        public void Parse(TextReader input)
        {
            string line;
            while ((line = input.ReadLine()) != null)
            {
                var newStat = new IagoStatistics();
                newStat.Parse(line);
                _iagoStatistics.Add(newStat);
            }
        }
        
        public void Analyze()
        {
            throw new NotImplementedException();
        }
    }

}