using System;
using System.Collections.Generic;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes.Charts;
using StatsReader;

namespace LaserPrinter.Graphs
{
    public class LineStackedGraph : Graph
    {
        public LineStackedGraph(GraphData graphData)
            : base(graphData)
        {
        }

        public override void Draw(Document document)
        {
            var chart = SetUp(ChartType.Line, document);

            var title=chart.TopArea.AddParagraph(GraphData.Title);
            title.Format.Font.Bold = true;

            var keyForKey = new List<Tuple<char, string>>();
            char seriesShortName = 'A';

            foreach (var seriesData in GraphData.SeriesData)
            {
                var series = chart.SeriesCollection.AddSeries();
                keyForKey.Add(new Tuple<char, string>( seriesShortName, seriesData.Name));
                series.Name = seriesShortName.ToString();
                series.Add(seriesData.Data.ToArray());

                seriesShortName++;
                if (seriesShortName > 'Z')
                {
                    throw new ArgumentException("More than 26 series supplied!");
                }
            }

            var bottomPar = chart.BottomArea.AddParagraph();
            bottomPar.AddFormattedText("Key for the Key", TextFormat.Bold);

            bottomPar = chart.BottomArea.AddParagraph();
            
            foreach (var tuple in keyForKey)
            {
                bottomPar.AddText(string.Format("{0} is {1}\n", tuple.Item1, tuple.Item2));
            }

            chart.XAxis.MinorTickMark = TickMarkType.None;
            chart.XAxis.MajorTickMark = TickMarkType.None;
            chart.XAxis.Title.Caption = "X-Axis";

            chart.YAxis.MajorTickMark = TickMarkType.Outside;
            chart.YAxis.Title.Caption = "Y-Axis";
            chart.YAxis.HasMajorGridlines = true;

            SetGlobalChartOptions(chart);

            document.LastSection.Add(chart);
        }
    }
}