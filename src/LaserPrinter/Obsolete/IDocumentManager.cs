using StatsReader;

namespace LaserPrinter.Obsolete
{
    public interface IDocumentManager
    {
        void CreateGraphSection(GraphData graphData);
        void AttachFileToDocument(string existingPdfFile, string updatedPdfFile, string attachmentFile);
        void EmbedFile(string pdf, string embeddedFile);
    }
}