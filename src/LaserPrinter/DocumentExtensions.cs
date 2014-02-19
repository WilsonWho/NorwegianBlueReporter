using System.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using iTextSharp.text.pdf;

namespace LaserPrinter
{
    public static class DocumentExtensions
    {
        public static void AttachFile(this Document document, string existingPdfFile, string updatedPdfFile, string attachmentFile)
         {
             var pdfReader = new PdfReader(existingPdfFile);
             var outStream = new FileStream(updatedPdfFile, FileMode.Create);

             var pdfStamper = new PdfStamper(pdfReader, outStream);
             var pdfWriter = pdfStamper.Writer;

             var pdfAttachment = PdfFileSpecification.FileEmbedded(pdfWriter, attachmentFile, attachmentFile, null);
             pdfStamper.AddFileAttachment(attachmentFile, pdfAttachment);
             pdfStamper.Close();
         }

        public static void SaveFile(this Document document, string fileName, string fileExtension)
        {
            string extension = string.Empty;

            if (!fileExtension.StartsWith("."))
            {
                extension = fileExtension.Insert(0, ".");
            }

            var renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always)
            {
                Document = document
            };

            renderer.RenderDocument();
            renderer.PdfDocument.Save(fileName + extension);
        }
    }
}