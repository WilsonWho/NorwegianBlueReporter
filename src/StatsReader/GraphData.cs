using System;
using System.Collections.Generic;

namespace StatsReader
{
    public enum GraphType
    {
        Bar,
        Line
    };

    public class SeriesData
    {
        public string Name { get; private set; }
        public List<Tuple<dynamic, dynamic>> Data { get; private set; } 
        
        public SeriesData(string name, List<Tuple<dynamic,dynamic>> data )
        {
            Name = name;
            Data = data;
        }
    }

    public class Graph
    {
        public string Title { get; private set; }
        public GraphType GraphType { get; private set; }
        public List<SeriesData> SeriesData { get; private set; }

        public Graph(string title, GraphType graphType, List<SeriesData> data)
        {
            Title = title;
            GraphType = graphType;
            SeriesData = data;
        }
    }
}
