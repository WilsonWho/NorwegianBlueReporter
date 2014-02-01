namespace LaserPrinter
{
    public interface IDocumentManager
    {
        void CreateGraphSection();
        void AttachFileToDocument(string existingPdfFile, string updatedPdfFile, string attachmentFile);
    }
}