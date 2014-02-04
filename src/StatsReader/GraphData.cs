using System;
using System.Collections.Generic;

namespace StatsReader
{
    public class SeriesData
    {
        public string Name { get; private set; }
        public List<Tuple<dynamic, dynamic>> Data { get; private set; } 
        
        SeriesData(string name, List<Tuple<dynamic,dynamic>> data )
        {
            Name = name;
            Data = data;
        }
    }

    public class Graph
    {
        public string Title { get; private set; }
        public List<SeriesData> SeriesData { get; private set; }

        Graph(string title, List<SeriesData>  data)
        {
            Title = title;
            SeriesData = data;
        }
    }
}
