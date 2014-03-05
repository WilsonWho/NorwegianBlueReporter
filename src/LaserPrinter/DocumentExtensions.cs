using System.IO;
using FSharp.Markdown.Pdf;
using LaserOptics.Common;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using LaserOptics;
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
                foreach (var graphData in analysisNote.GraphData)
                {
                    var graph = GraphFactory.CreateGraph(graphData);
                    graph.Draw(document);
                }
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

        public static void AttachAllFiles(this Document document, string existingPdfFile, string updatedPdfFile, string path)
        {
            var files = Directory.GetFiles(path);
            document.AttachFiles(existingPdfFile, updatedPdfFile, files);
        }

        public static void AttachAllFiles(this Document document, string existingPdfFile, string updatedPdfFile, string path, string searchPattern)
        {
            var files = Directory.GetFiles(path, searchPattern);
            document.AttachFiles(existingPdfFile, updatedPdfFile, files);
        }

        public static void AttachAllFiles(this Document document, string existingPdfFile, string updatedPdfFile, string path, string searchPattern, SearchOption searchOption)
        {
            var files = Directory.GetFiles(path, searchPattern, searchOption);
            document.AttachFiles(existingPdfFile, updatedPdfFile, files);
        }

        private static void AttachFiles(this Document document, string existingPdf, string updatedPdf, string[] files)
        {
            foreach (var file in files)
            {
                document.AttachFile(existingPdf, updatedPdf, file);
            }
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