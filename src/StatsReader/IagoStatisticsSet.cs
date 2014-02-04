using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using OpenCvSharp;

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
            //Cv.KMeans2(points, maxClusters, clusters, new CvTermCriteria(10, 1.0));

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
            foreach (var stat in _iagoStatistics)
            {
                stat.Analyze();
            }

            var analyzers = new CommonAnalysis();
            _analysisScratchPad.AllHeaders = analyzers.FindAllHeaders(_iagoStatistics);

        }
    }

}