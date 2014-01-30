using MigraDoc.DocumentObjectModel;

namespace LaserPrinter
{
    public interface IDocumentManager
    {
        Document CreateDocument();
        void CreateGraphSection(Document document);
    }
}