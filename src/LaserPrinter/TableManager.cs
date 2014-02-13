using MigraDoc.DocumentObjectModel;

namespace LaserPrinter
{
    public class TableManager : ITableManager
    {
        public void DefineTable(Document document)
        {
            var table = document.LastSection.AddTable();
            table.Style = "Table";
            table.Borders.Color = Colors.IndianRed;
            table.Borders.Width = 0.5;
            table.Borders.Left.Width = 0.25;
            table.Borders.Right.Width = 0.25;

            for (int i = 0; i < 10; i++)
            {
                var column = table.AddColumn();
                column.Format.Alignment = ParagraphAlignment.Center;
            }

            for (int i = 0; i < 3; i++)
            {
                var row = table.AddRow();
                row.Format.Alignment = ParagraphAlignment.Center;
                row.Shading.Color = Colors.Honeydew;
            }

            table.Rows[0].Cells[0].Shading.Color = Colors.Lavender;
        }
    }
}