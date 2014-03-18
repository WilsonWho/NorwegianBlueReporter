using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NorwegianBlue.IagoIntegration.Analysis
{
    public class IagoSampleSetAnalysis
    {
        private readonly List<String> _requestLatencySeries;
        
        public IagoSampleSetAnalysis()
        {
            // ToDo: pull from object config 
            _requestLatencySeries = new List<string>
                {
                    "client/request_latency_ms_average",
                    "client/request_latency_ms_maximum",
                    "client/request_latency_ms_minimum",
                    //"client/request_latency_ms_p50",
                    //"client/request_latency_ms_p90",
                    //"client/request_latency_ms_p95"
                };
        }

        public void IagoRequestLatencySummary(ISampleSetAnalysis statSet)
        {
            var averages = statSet.AnalysisScratchPad.Averages;

            // Latency summary

            var seriesData = new List<SeriesData>();
            var notes = new StringBuilder();

            // get the stat-by-stat values
            foreach (var seriesName in _requestLatencySeries)
            {
                var data = new List<double>();
                foreach (var stats in statSet)
                {
                    if (stats.ContainsKey(seriesName))
                    {
                        data.Add(stats[seriesName]);
                    }
                    else
                    {
                        data.Add(data.Count > 0 ? data[data.Count - 1] : 0d);
                        notes.AppendFormat("{0} Missing value for {1}\n", stats.TimeStamp, seriesName);
                    }
                }

                seriesData.Add(new SeriesData(seriesName, data));
            }

            // add the averages from the entire dataset for each value
            var avgRequestLatencySeries = new List<string>();
            foreach (var series in _requestLatencySeries)
            {
                var avgSeriesName = series + " set average";
                avgRequestLatencySeries.Add(avgSeriesName);
                var data = new List<double>();
                double avg = averages[series];
                for (int i = 0; i < statSet.Count; i++)
                {
                    data.Add(avg);
                }

                seriesData.Add(new SeriesData(avgSeriesName, data));
            }

            var labels =
                statSet.Select(stats => stats.TimeStamp.ToString(CultureInfo.InvariantCulture)).ToList();

            var requestLatencyGraphs = new List<GraphData>();
            var requestLatencyGraph = new GraphData("Request Latencies", labels, true, LegendPositionEnum.Footer, false,
                                                    GraphType.Line, seriesData);
            requestLatencyGraphs.Add(requestLatencyGraph);

            var analysisSummary = new StringBuilder(
                @"# Average Request Latency

The following graphs the average client request statistics for each minute of the test duration.
"
                );

            if (notes.Length > 0)
            {
                analysisSummary.Append(
                    @"*Notes*:
Missing data is replaced either with a 0 when there is no preceeding data or the previous value.

*Time stamps with Missing Data*
"
                    );
                analysisSummary.Append(notes);

            }

            // Create Detailed graphs for each Client Request Latency
            foreach (var latencySeries in _requestLatencySeries)
            {
                seriesData = new List<SeriesData>();
                var data = new List<double>();

                foreach (var stats in statSet)
                {
                    if (stats.ContainsKey(latencySeries))
                    {
                        data.Add(stats[latencySeries]);
                    }
                    else
                    {
                        data.Add(data.Count > 0 ? data[data.Count - 1] : 0d); 
                        // don't need to add anything to the notes; would've alread been done when we generated the combined log graph above
                    }
                }
                seriesData.Add(new SeriesData(latencySeries, data));

                requestLatencyGraph = new GraphData(latencySeries, labels, true, LegendPositionEnum.Footer, false,
                                          GraphType.Line, seriesData);
                requestLatencyGraphs.Add(requestLatencyGraph);
            }

            statSet.AddAnalysisNote(new AnalysisNote("Request Latencies", analysisSummary.ToString(),
                                                     requestLatencyGraphs));
        }
    }
}
