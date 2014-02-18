using System.Collections.Generic;
using System.Linq;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using StatsReader;

namespace LaserPrinter.Graphs
{
    public class ColorTableGraph : Graph
    {
        public ColorTableGraph(GraphData graphData) : base(graphData)
        {
        }

        public override void Draw(Document document)
        {
            // Colors copied from http://stackoverflow.com/questions/470690/how-to-automatically-generate-n-distinct-colors
            var colors = new List<Color>
                {
                    new Color(0xFFFFB300), //Vivid Yellow
                    new Color(0xFF803E75), //Strong Purple
                    new Color(0xFFFF6800), //Vivid Orange
                    new Color(0xFFA6BDD7), //Very Light Blue
                    new Color(0xFFC10020), //Vivid Red
                    new Color(0xFFCEA262), //Grayish Yellow
                    new Color(0xFF817066), //Medium Gray

                    //The following will not be good for people with defective color vision
                    new Color(0xFF007D34), //Vivid Green
                    new Color(0xFFF6768E), //Strong Purplish Pink
                    new Color(0xFF00538A), //Strong Blue
                    new Color(0xFFFF7A5C), //Strong Yellowish Pink
                    new Color(0xFF53377A), //Strong Violet
                    new Color(0xFFFF8E00), //Vivid Orange Yellow
                    new Color(0xFFB32851), //Strong Purplish Red
                    new Color(0xFFF4C800), //Vivid Greenish Yellow
                    new Color(0xFF7F180D), //Strong Reddish Brown
                    new Color(0xFF93AA00), //Vivid Yellowish Green
                    new Color(0xFF593315), //Deep Yellowish Brown
                    new Color(0xFFF13A13), //Vivid Reddish Orange
                    new Color(0xFF232C16), //Dark Olive Green
                };

            // figure out dimensions
            var defaultPageSetup = document.DefaultPageSetup;
            Unit pageWidth = defaultPageSetup.PageWidth - defaultPageSetup.RightMargin - defaultPageSetup.LeftMargin;
            int columnCount = GraphData.Labels.Count;
            var labelWidth = new Unit(2, UnitType.Centimeter);
            Unit cellWidth = (pageWidth - labelWidth) / (columnCount + 1);
   
            var table = document.LastSection.AddTable();
            table.Style = "Table";
            table.Rows.Height = "2cm";
            table.Rows.Alignment = RowAlignment.Center;
            table.Rows.VerticalAlignment = VerticalAlignment.Center;
            table.AddColumn(labelWidth);

            foreach (var label in GraphData.Labels)
            {
                var cell = table.AddColumn();
                cell.Width = cellWidth;
            }

            foreach (var series in GraphData.SeriesData)
            {
                var row = table.AddRow();
                row.Format.Alignment = ParagraphAlignment.Center;

                var labelText = new Paragraph
                {
                    Format = new ParagraphFormat
                    {
                        Font = new Font
                        {
                            Size = "5pt"
                        }
                    },

                };

                labelText.AddText(series.Name);
                row.Cells[0].Add(labelText);

                var max = series.Data.Max();
                var min = series.Data.Min();
                var range = max - min;

                for (int cell = 0; cell < series.Data.Count; cell++)
                {
                    var datum = series.Data[cell];
                    var colorOffset = (int) (((datum - min)/range) * (colors.Count - 1));
                    row[cell+1].Shading.Color = colors[colorOffset];
                }
            }

            table.SetEdge(0, 0, columnCount + 1, GraphData.SeriesData.Count, Edge.Horizontal, BorderStyle.Single, 1.3, Colors.Black);
            table.Format.Alignment = ParagraphAlignment.Justify;

        }
    }
}