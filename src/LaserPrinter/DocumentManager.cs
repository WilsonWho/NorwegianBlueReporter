using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Extensions.Html;
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
            var section = new Section();
            _document.Sections.Add(section.AddMarkdown(markdown));
        }

        public void AddHtml(string html)
        {
            var section = new Section();
            _document.Sections.Add(section.AddHtml(html));
        }

        public void CreateGraphSection()
        {
            _graphManager.DefineChart(_document);
        }

        public void EmbedFile(string pdf, string embed)
        {
            var pdfStream = new FileStream(pdf, FileMode.Append, FileAccess.Write);
            //var pdfBinaryReader = new BinaryReader(pdfStream);

            //var embedStream = new FileStream(embed, FileMode.Open, FileAccess.ReadWrite);
            //var embedBinaryReader = new BinaryReader(embedStream);

            var embeddedFile = File.OpenRead(embed);

            var rawBinaryData = ConstructRawBinaryData(52, 0, embeddedFile, "/Type /EmbeddedFile", Decode.Flate);

        }

        private string ConstructRawBinaryData(int index, int version, FileStream embeddedFile, string entryType, Decode decodeType)
        {
            //var length = (int) binaryReader.BaseStream.Length;
            //var buffer = new byte[length];
            //DeflateStream encodedData = null;
            var builder = new StringBuilder();

            switch (decodeType)
            {
                case Decode.AsciiHex:
                    //encodedData = null;
                    throw new NotImplementedException("This decode type is currently not supported ...");
                case Decode.Flate:
                    //encodedData = new DeflateStream(binaryReader, CompressionMode.Compress);
                    //binaryReader.Read(buffer, 0, length);
                    break;
            }

            builder.Append("\n");
            builder.Append(embeddedFile.Length);
            builder.Append(string.Format("{0} {1} obj\n<<\n /Length {2}\n", index, version, embeddedFile.Length));
            builder.Append(string.Format(" /Filter {0}\n", decodeType));

            if (!string.IsNullOrEmpty(entryType))
            {
                builder.Append(string.Format(" {0}\n", entryType));
            }

            builder.Append(">>\nstream\n");

            //int compressedRawBinaryData = encodedData.Read(buffer, 0, (int) encodedData.Length);
            //builder.Append(compressedRawBinaryData);

            builder.Append(embeddedFile);
            builder.Append("\nendstream\nendobj\n");

            return builder.ToString();
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