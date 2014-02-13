using System.Collections.Generic;
using MigraDoc.DocumentObjectModel;
using StatsReader;

namespace LaserPrinter
{
    public interface IGraphManager
    {
        void DefineComboGraph(Document document, List<SeriesData> seriesDataList);
        void DefineColumnGraph(Document document, List<SeriesData> seriesDataList);
        void DefineColumnStackedGraph(Document document, List<SeriesData> seriesDataList);
        void DefineBarGraph(Document document, List<SeriesData> seriesDataList);
        void DefineExplodedPieGraph(Document document, List<SeriesData> seriesDataList);
    }
}