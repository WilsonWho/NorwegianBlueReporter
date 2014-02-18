using System.Collections.Generic;
using StatsReader;
using GraphType = LaserPrinter.Obsolete.GraphType;

namespace LaserPrinter
{
    public interface IDocumentManager
    {
        void CreateGraphSection(GraphType graphType, List<SeriesData> seriesDataList);
        void CreateTableSection();
        void AttachFileToDocument(string existingPdfFile, string updatedPdfFile, string attachmentFile);
        void EmbedFile(string pdf, string embeddedFile);
    }
}