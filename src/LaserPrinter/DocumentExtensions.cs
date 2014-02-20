using System.IO;
using FSharp.Markdown.Pdf;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using StatsReader;
using iTextSharp.text.pdf;

namespace LaserPrinter
{
    public static class DocumentExtensions
    {
        public static void AppendMarkdown(this Document document, string markdown, Section section = null)
        {
            if (section == null)
            {
                section = document.LastSection;
            }

            MarkdownPdf.AddMarkdown(document, section, markdown);
        }

        public static void AddMarkdown(this Document document, string markdown)
        {
            document.AppendMarkdown(markdown, document.AddSection());
        }

        public static void AppendAnalysisNote(this Document document, AnalysisNote analysisNote, Section section = null)
        {
            bool summaryPresent = !string.IsNullOrEmpty(analysisNote.Summary);
            bool graphPresent = (analysisNote.GraphData != null);

            if (section == null)
            {
                section = document.LastSection;
            }

            if (summaryPresent)
            {
                document.AppendMarkdown(analysisNote.Summary, section);
            }

            if (graphPresent)
            {
                var graph = GraphFactory.CreateGraph(analysisNote.GraphData);
                graph.Draw(document);
            }
        }

        public static void AttachFile(this Document document, string existingPdfFile, string updatedPdfFile,
                                      string attachmentFile)
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