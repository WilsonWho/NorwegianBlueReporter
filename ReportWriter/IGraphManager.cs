using MigraDoc.DocumentObjectModel;

namespace ReportWriter
{
    public interface IGraphManager
    {
        void DefineChart(Document document);
    }
}