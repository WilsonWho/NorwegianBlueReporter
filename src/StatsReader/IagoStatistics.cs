using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatsReader
{
    class IagoStatistics : IStatistics, IStatisticsValues, IStatisticsAddNote 
    {
        private Dictionary<String, String> _stats = new Dictionary<string, string>();
        private Dictionary<String, String> _analysisNotes = new Dictionary<string, string>(); 
        public DateTime TimeStamp { get ; set; }

        ReadOnlyDictionary<String, String> IStatisticsValues.Stats
        {
            get { return Utils.ReadOnlyDictionaryWithCopiedValues(_stats);}  
        }

        ReadOnlyDictionary<string, string> IStatisticsValues.AnalysisNotes
        {
            get { return Utils.ReadOnlyDictionaryWithCopiedValues(_analysisNotes); }
        }

        public void Parse(String input)
        {
            throw new NotImplementedException();
        }

        public void AddAnalysisNote(string analysisName, string note)
        {
            _analysisNotes[analysisName] = note;
        }
    }
}
