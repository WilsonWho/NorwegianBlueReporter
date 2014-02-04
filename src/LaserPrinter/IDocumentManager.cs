namespace LaserPrinter
{
    public interface IDocumentManager
    {
        void CreateGraphSection();
        void EmbedFile(string pdf, string embed);
        void AttachFileToDocument(string existingPdfFile, string updatedPdfFile, string attachmentFile);
    }
}