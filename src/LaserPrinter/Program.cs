using System.Collections.Generic;
using LaserOptics;
using MigraDoc.DocumentObjectModel;

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

            var data = new List<SeriesData>
                {
                    new SeriesData("Series1", new List<double> {15.0, 26.0, 52.0}),
                    new SeriesData("Series2", new List<double> {1.0, 36.0, 142.0}),
                    new SeriesData("Series3", new List<double> {29.0, 165.0, 18.0}),
                    new SeriesData("Series4", new List<double> {37.0, 10.0, 272.0}),
                    new SeriesData("Series5", new List<double> {129.0, 89.0, 16.0}),
                };

            var labels = new List<string>
                {
                    "Series1 Label",
                    "Series2 Label",
                    "Series3 Label",
                };

            var graphData = new GraphData("OxyPlot", labels, false, LegendPositionEnum.Left, false, GraphType.LineStacked, data);
            var hi = GraphFactory.CreateGraph(graphData);
            hi.Draw(document);

            // TODO -- changed the constructor for DocumentManager; so the next part is probably busted
            //var documentManager = new DocumentManager();
            //document.AddSection();

            //var seriesData = new List<SeriesData>
            //    {
            //        new SeriesData("AHHH", new List<double> {1, 2, 3})
            //    };


            //var graphData = new GraphData("WICKED COLOR TABLE GRAPH THING", null, false, LegendPositionEnum.Left, false,
            //                              GraphType.None, seriesData);

            //var tableGraph = new ColorTableGraph(graphData);
            //tableGraph.Draw(document);


            //const string fileName = "Experiment Alpha.pdf";
            //documentManager.SaveAsPdf(fileName);

            //const string newFileName = "Experiment Beta.pdf";
            //documentManager.AttachFileToDocument(fileName, newFileName, "TestCSV.csv");

            //const string embed = "TestCSV.csv";
            //documentManager.EmbedFile(fileName, embed);

            //Process.Start(fileName);
        }
    }
}
