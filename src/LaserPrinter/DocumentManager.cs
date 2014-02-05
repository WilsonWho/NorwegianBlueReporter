using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Extensions.Html;
using MigraDoc.Rendering;
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

        // TODO: Add set of statistics as another parameter and (?)header info(?)
        public void CreateGraphSection(GraphType graphType)
        {
            _document.AddSection();

            switch (graphType)
            {
                case GraphType.Combo:
                    // Extract required fields from the set of statistics for the line combo graph and pass it in as a parameter
                    // Pass in required header info as well(?)
                    _graphManager.DefineComboGraph(_document);
                    break;
                case GraphType.ColumnStacked:
                    // Extract required fields from the set of statistics for the column stacked graph and pass it in as a parameter
                    // Pass in required header info as well(?)
                    _graphManager.DefineColumnStackedGraph(_document);
                    break;
                default:
                    throw new InvalidEnumArgumentException("Specified graph type is not supported ...");
            }
        }

        public void SaveAsPdf(string fileName)
        {
            var renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always)
            {
                Document = _document
            };

            renderer.RenderDocument();
            renderer.PdfDocument.Save(fileName);
        }

        // Need to specify existing created by SaveAsPdf() and a new PDF file to write to. Due to the nature of PDFs, it seems like iTextSharp cannot
        // just insert an attachment to an already created PDF file. Instead, they need to generate a completely new one and copy the contents while
        // adding any new additions.
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

        // TODO: Code below this is a work in progress...
        public void AddMarkDown(string markdown)
        {
            var section = new Section();
            _document.Sections.Add(section.AddMarkdown(markdown));
        }

        public void AddHtml(string html)
        {
            var section = new Section();
            _document.Sections.Add(section.AddHtml(html));
        }

        public void EmbedFile(string pdfFile, string embeddedFile)
        {
            var embeddedFileStream = new FileStream(embeddedFile, FileMode.Open, FileAccess.ReadWrite);
            ImplantData(pdfFile, 52, 0, "/Type /EmbeddedFile", embeddedFileStream, Decode.Flate);
        }

        private void ImplantData(string pdfFile, int index, int version, string entryType, FileStream embeddedFile, Decode decodeType)
        {
            bool dataAdded = false;
            const string pattern = @"\d+ \d+ obj";
            var regex = new Regex(pattern);

            var encoding = new UTF8Encoding();
            string[] result = File.ReadAllLines(pdfFile, Encoding.Default);

            var fs = new FileStream("SomethingNew", FileMode.Create);
            var bw = new BinaryWriter(fs, Encoding.UTF8);

            foreach (string t in result)
            {
                var matchFound = regex.IsMatch(t);

                //if (matchFound && !dataAdded)
                //{
                //    dataAdded = true;
                //    var builder = new StringBuilder();
                //    builder.Append("\n");
                //    builder.Append(embeddedFile.Length);
                //    builder.Append(string.Format("{0} {1} obj\n<<\n /Length {2}\n", index, version, embeddedFile.Length));
                //    builder.Append(string.Format(" /Filter {0}\n", decodeType));

                //    if (!string.IsNullOrEmpty(entryType))
                //    {
                //        builder.Append(string.Format(" {0}\n", entryType));
                //    }

                //    builder.Append(">>\nstream\n");

                //    byte[] startBuffer = encoding.GetBytes(builder.ToString());
                //    bw.Write(startBuffer);

                //    var gZipCompressed = ConstructRawBinaryData(embeddedFile, decodeType);
                //    int length = (int) gZipCompressed.BaseStream.Length;
                //    var attachmentBytes = new byte[length];
                //    //gZipCompressed.Seek(0, SeekOrigin.Begin);
                //    //gZipCompressed.CopyTo(fs);
                //    //gZipCompressed.Write(attachmentBytes, 0, attachmentBytes.Length);
                    
                //    const string endObj = "\nendstream\nendobj\n";
                //    byte[] endBuffer = encoding.GetBytes(endObj);
                //    bw.Write(endBuffer);
                //}

                byte[] buffer = encoding.GetBytes(t);
                bw.Write(buffer + "\n");
            }

            bw.Close();
        }

        private GZipStream ConstructRawBinaryData(FileStream embeddedFile, Decode decodeType)
        {
            var length = (int)embeddedFile.Length;
            var buffer = new byte[length];
            DeflateStream encodedData;
            GZipStream gZipStream = null;

            switch (decodeType)
            {
                case Decode.AsciiHex:
                    //encodedData = null;
                    throw new NotImplementedException("This decode type is currently not supported ...");
                case Decode.Flate:
                    //encodedData = new DeflateStream(embeddedFile, CompressionMode.Compress);
                    //encodedData.Read(buffer, 0, length);
                    
                    gZipStream = new GZipStream(embeddedFile, CompressionMode.Compress);
                    //gZipStream.Read(buffer, 0, length);

                    //embeddedFile.Read(buffer, 0, length);
                    
                    break;
            }

            return gZipStream;
        }
    }
}