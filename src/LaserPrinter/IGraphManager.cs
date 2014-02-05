using MigraDoc.DocumentObjectModel;

namespace LaserPrinter
{
    public interface IGraphManager
    {
        void DefineComboGraph(Document document);
        void DefineColumnStackedGraph(Document document);
    }
}