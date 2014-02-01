using System.Diagnostics;
using LaserPrinter.Test;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace LaserPrinter
{
    class Program
    {
        static void Main(string[] args)
        {
            var document = new Document
            {
                Info =
                {
                    Title = "Experiment Alpha",
                    Subject = "Splicing chart DNAs",
                    Author = "Dr. Wilson"
                }
            };

            const string markdown = @"
                                        # This is a heading

                                        This is some **bold** ass text with a [link](http://www.google.com).

                                        - List Item 1
                                        - List Item 2
                                        - List Item 3

                                        Pretty cool huh?
                                    ";

            //var entry = new Entry();
            //entry.Start();

            var documentManager = new DocumentManager(document);
            documentManager.CreateGraphSection();
            documentManager.AddMarkDown(markdown);

            var renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always)
                {
                    Document = document
                };

            renderer.RenderDocument();

            const string fileName = "ExperimentAlpha";
            renderer.PdfDocument.Save(fileName);

            const string updatedFileName = "ExperimentBeta";
            documentManager.AttachFileToDocument(fileName, updatedFileName, "TestCSV.csv");

            Process.Start(updatedFileName);
        }
    }
}
