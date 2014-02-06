using System;
using System.Collections.Generic;
using MigraDoc.DocumentObjectModel;
using StatsReader;

namespace LaserPrinter
{
    public interface IGraphManager
    {
        void DefineComboGraph(Document document);
        void DefineColumnStackedGraph(Document document, List<SeriesData> seriesDataList);
    }
}