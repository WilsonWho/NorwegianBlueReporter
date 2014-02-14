using System.Collections.Generic;
using StatsReader;

namespace LaserPrinter.Obsolete
{
    public interface IDocumentManager
    {
        void CreateGraphSection(GraphType graphType, List<SeriesData> seriesDataList);
        void CreateTableSection();
        void AttachFileToDocument(string existingPdfFile, string updatedPdfFile, string attachmentFile);
        void EmbedFile(string pdf, string embeddedFile);
    }
}