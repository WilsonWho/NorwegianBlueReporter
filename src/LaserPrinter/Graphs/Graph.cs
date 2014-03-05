using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;

namespace LaserPrinter.Graphs
{
    public abstract class Graph
    {
        protected abstract void SetGlobalChartOptions(Chart chart);
        public abstract void Draw(Document document);
    }
}