namespace LaserPrinter
{
    public interface IDocumentManager
    {
        void CreateGraphSection(GraphType graphType);
        void AttachFileToDocument(string existingPdfFile, string updatedPdfFile, string attachmentFile);
        void EmbedFile(string pdf, string embeddedFile);
    }
}