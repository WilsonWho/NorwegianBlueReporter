using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using LaserPrinter.Graphs;
using LaserPrinter.Obsolete;
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

            // IGNORE
            // ---------------------------------------------------------------------------------------------------------------------------------------------------

            const string markdown = @"
                                        # This is a heading

                                        This is some **bold** ass text with a [link](http://www.google.com).

                                        - List Item 1
                                        - List Item 2
                                        - List Item 3

                                        Pretty cool huh?
                                    ";

            const string html = @"
                                    <h1>This is a heading</h1>

                                    <p>This is some **bold** ass text with a <a href='http://www.google.com'>link</a>.<p>

                                    <ul>
                                        <li>List Item 1</li>
                                        <li>List Item 2</li>
                                        <li>List Item 3</li>
                                    </ul>

                                    <p>Pretty cool huh?</p>
                                ";

            // ---------------------------------------------------------------------------------------------------------------------------------------------------

            var documentManager = new DocumentManager(document);
            //documentManager.CreateGraphSection(GraphType.Column, null);
            //documentManager.CreateGraphSection(GraphType.Bar, null);
            //documentManager.CreateGraphSection(GraphType.ExplodedPie, null);
            //documentManager.AddMarkDown(markdown);
            //documentManager.AddHtml(html);

            //documentManager.CreateTableSection();
            document.AddSection();

            var tuples = new List<Tuple<string, double>>
                {
                    new Tuple<string, double>("Julian", 1),
                    new Tuple<string, double>("Angel", 3),
                    new Tuple<string, double>("Wilson", 2)
                };

            var columnGraph = new ColumnGraph("WICKED GRAPH", false, Graph.LegendPositionEnum.Left, false, tuples);

            var lineData = new List<double>
                {
                    3.0,
                    10.2,
                    5.6,
                    17.1,
                    8.4
                };

            var lineGraph = new LineGraph("WICKED LINE GRAPH", false, Graph.LegendPositionEnum.Right, false, lineData);

            var comboLineData = new List<Tuple<string, List<double>>>
                {
                    new Tuple<string, List<double>>("Series 1", new List<double> {1, 2, 3, 4, 5, 6}),
                    new Tuple<string, List<double>>("Series 2", new List<double> {1, 5, 7, 8, 9, 12}),
                    new Tuple<string, List<double>>("Series 3", new List<double> {4, 5, 6, 9, 10, 16})
                };

            var lineStackedGraph = new LineStackedGraph("WICKED LINE STACKED GRAPH", false, Graph.LegendPositionEnum.Footer, false, comboLineData);

            var tableGraph = new TableGraph("WICKED TABLE GRAPH", false, Graph.LegendPositionEnum.Footer, false);

            document.LastSection.AddParagraph("HELLO PASERUASDF");
            columnGraph.Draw(document);
            lineGraph.Draw(document);
            lineStackedGraph.Draw(document);
            tableGraph.Draw(document);


            const string fileName = "Experiment Alpha.pdf";
            documentManager.SaveAsPdf(fileName);

            //const string newFileName = "Experiment Beta.pdf";
            //documentManager.AttachFileToDocument(fileName, newFileName, "TestCSV.csv");

            //const string embed = "TestCSV.csv";
            //documentManager.EmbedFile(fileName, embed);

            Process.Start(fileName);
        }
    }
}
