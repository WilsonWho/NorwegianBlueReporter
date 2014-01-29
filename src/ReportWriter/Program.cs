using System.Diagnostics;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace ReportWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            var documentManager = new DocumentManager();
            Document document = documentManager.CreateDocument();
            documentManager.CreateGraphSection(document);

            var renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always)
                {
                    Document = document
                };

            renderer.RenderDocument();

            const string fileName = "ExperimentAlpha";
            renderer.PdfDocument.Save(fileName);

            Process.Start(fileName);
        }
    }
}
