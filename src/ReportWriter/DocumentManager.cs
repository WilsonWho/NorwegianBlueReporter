using MigraDoc.DocumentObjectModel;

namespace LaserPrinter
{
    public class DocumentManager : IDocumentManager
    {
        private readonly GraphManager _graphManager;

        public DocumentManager()
        {
            _graphManager = new GraphManager();
        }

        public Document CreateDocument()
        {
            // TODO: allow field input
            var document = new Document
                {
                    Info =
                        {
                            Title = "Experiment Alpha",
                            Subject = "Splicing chart DNAs",
                            Author = "Dr. Wilson"
                        }
                };

            //TODO: perform certain preloaded customizations to the documentation

            return document;
        }

        public void CreateGraphSection(Document document)
        {
            _graphManager.DefineChart(document);
        }
    }
}