using System.Drawing;
using System.IO;
using MigraDoc.DocumentObjectModel;
using OxyPlot;
using OxyPlot.WindowsForms;

namespace NorwegianBlue.Util.Pdf
{
    public class OxyPlotGraph
    {
        public void SaveToMigraDocPdf(Document document, PlotModel plotModel)
        {
            var fileName = ExportPng(plotModel);

            var image = document.LastSection.AddImage(fileName);
            image.Width = Unit.FromPoint(400);
        }

        private string ExportPng(PlotModel plotModel)
        {
            var tmp = Path.GetTempFileName();
            PngExporter.Export(plotModel, tmp, 800, 800, Brushes.White);

            return tmp;
        }
    }
}