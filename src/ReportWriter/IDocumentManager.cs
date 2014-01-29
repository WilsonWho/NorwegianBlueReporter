using MigraDoc.DocumentObjectModel;

namespace ReportWriter
{
    public interface IDocumentManager
    {
        Document CreateDocument();
        void CreateGraphSection(Document document);
    }
}