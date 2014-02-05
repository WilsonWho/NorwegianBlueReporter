using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using OpenCvSharp;

namespace StatsReader
{
    class IagoStatisticsSet : IStatisticsSet, IStatisticsSetAnalysis
    {
        private readonly dynamic _analysisScratchPad = new ExpandoObject();
        private readonly List<IagoStatistics> _iagoStatistics = new List<IagoStatistics>();
        private ReadOnlyCollection<IStatisticsValues> _roIagoStatistics ;
        private readonly List<AnalysisNote> _analysisNotes = new List<AnalysisNote>();
        private ReadOnlyCollection<AnalysisNote> _roAnalysisNote;

        public ReadOnlyCollection<IStatisticsValues> Statistics 
        {
            get { return _roIagoStatistics ?? ( _roIagoStatistics = new ReadOnlyCollection<IStatisticsValues>(_iagoStatistics.Cast<IStatisticsValues>().ToList())); }
        }

        public ReadOnlyCollection<AnalysisNote> AnalysisNotes
        {
            get { return _roAnalysisNote ?? ( _roAnalysisNote = new ReadOnlyCollection<AnalysisNote>(_analysisNotes)); }
        }

        public dynamic AnalysisScratchPad { get { return _analysisScratchPad; } }

        public void Parse(TextReader input)
        {
            //Cv.KMeans2(points, maxClusters, clusters, new CvTermCriteria(10, 1.0));

            string line;
            while ((line = input.ReadLine()) != null)
            {
                var newStat = new IagoStatistics();
                newStat.Parse(line);
                _iagoStatistics.Add(newStat);
            }
        }

        public void Analyze(IEnumerable<SetAnalyzer> setAnalyzers , IEnumerable<StatAnalyzer> statAnalyzers)
        {
            foreach (var analyzer in setAnalyzers)
            {
                analyzer.Invoke(this);
            }

            foreach (var stat in _iagoStatistics)
            {
                foreach (var analyzer in statAnalyzers)
                {
                    analyzer.Invoke(this, stat);
                }
            }
        }

        public void AddAnalysisNote(AnalysisNote note)
        {
            _analysisNotes.Add(note);
        }

    }

}