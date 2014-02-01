using System.IO;
using MigraDoc.DocumentObjectModel;
using iTextSharp.text.pdf;
using MigraDoc.Extensions.Markdown;

namespace LaserPrinter
{
    public class DocumentManager : IDocumentManager
    {
        private readonly Document _document;
        private readonly GraphManager _graphManager;

        public DocumentManager(Document document)
        {
            _document = document;
            _graphManager = new GraphManager();
        }

        public void AddMarkDown(string markdown)
        {
            _document.LastSection.AddMarkdown(markdown);
        }

        public void CreateGraphSection()
        {
            _graphManager.DefineChart(_document);
        }

        public void AttachFileToDocument(string existingPdfFile, string updatedPdfFile, string attachmentFile)
        {
            var pdfReader = new PdfReader(existingPdfFile);
            var outStream = new FileStream(updatedPdfFile, FileMode.Create);

            var pdfStamper = new PdfStamper(pdfReader, outStream);
            var pdfWriter = pdfStamper.Writer;

            var pdfAttachment = PdfFileSpecification.FileEmbedded(pdfWriter, attachmentFile, attachmentFile, null);
            pdfStamper.AddFileAttachment(attachmentFile, pdfAttachment);
            pdfStamper.Close();
        }
    }
}