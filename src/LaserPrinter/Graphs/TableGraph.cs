using MigraDoc.DocumentObjectModel;
using StatsReader;

namespace LaserPrinter.Graphs
{
    public class TableGraph : Graph
    {
        public TableGraph(GraphData graphData) : base(graphData)
        {
        }

        public override void Draw(Document document)
        {
            // figure out dimensions
            var defaultPageSetup = document.DefaultPageSetup;
            var columnWidth = defaultPageSetup.PageWidth - defaultPageSetup.RightMargin - defaultPageSetup.LeftMargin;

   
            var table = document.LastSection.AddTable();
            table.Style = "Table";
            table.Borders.Color = Colors.IndianRed;
            table.Borders.Width = 0.5;
            table.Borders.Left.Width = 0.25;
            table.Borders.Right.Width = 0.25;

            for (int i = 0; i < 10; i++)
            {
                var column = table.AddColumn();
                column.Width = (10d / 24d) * 72;
                column.Format.Alignment = ParagraphAlignment.Center;
            }

            for (int i = 0; i < 3; i++)
            {
                var row = table.AddRow();
                row.Format.Alignment = ParagraphAlignment.Center;
                row.Shading.Color = Colors.MidnightBlue;
            }

            table.Rows[0].Cells[0].Shading.Color = Colors.MintCream;
        }
    }
}