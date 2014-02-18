using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StatsReader
{
    public enum GraphType
    {
        None,
        Combo,
        Column,
        ColumnStacked,
        Bar,
        ExplodedPie,
        ColorTableGraph
    };

    public enum LegendPositionEnum
    {
        Left,
        Right,
        Footer
    }

    public class SeriesData
    {
        public string Name { get; private set; }

        public List<double> Data { get; private set; }
        
        public SeriesData(string name, List<double> data)
        {
            Name = name;
            Data = data;
        }
    }

    public class GraphData
    {

        public string Title { get; private set; }
        public List<string> Labels { get; private set; }
        public bool HasLegend { get; private set; }
        public LegendPositionEnum LegendPosition { get; private set; }
        public bool HasDataLabel { get; private set; }
        public GraphType GraphType { get; private set; }
        public List<SeriesData> SeriesData { get; private set; }

        public GraphData(string title, List<string> Labels, bool hasLegend, LegendPositionEnum legendPosition, bool hasDataLabel, GraphType graphType, List<SeriesData> data)
        {
            if (data == null || data.Count == 0)
            {
                throw new ArgumentException("No data present ...");
            }

            for (int i = 1; i < data.Count; i++)
            {
                if (data[i].Data.Count != Labels.Count)
                {
                    throw new ArgumentException("Label counts are not equal ...");
                }
            }

            Title = title;
            HasLegend = hasLegend;
            LegendPosition = legendPosition;
            HasDataLabel = hasDataLabel;
            GraphType = graphType;
            SeriesData = data;
        }
    }
}
