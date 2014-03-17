using AutoMapper;
using Microsoft.WindowsAzure.Management.WebSites.Models;
using NorwegianBlue.Azure.DTOs;
using NorwegianBlue.Azure.DTOs.WebSiteGetHistoricalUsageMetricsResponse;

namespace NorwegianBlue.Azure.Utilities
{
    public static class Maps
    {
        public static void Init()
        {
            CreateWebSiteGetHistoricalUsageMetricsResponseMapping();
        }

        private static void CreateWebSiteGetHistoricalUsageMetricsResponseMapping()
        {
            Mapper.CreateMap<WebSiteGetHistoricalUsageMetricsResponse, AzureGetHistoricalUsageMetricsResponse>()
                  .ForMember(custom => custom.UsageMetrics,
                             azure => azure.MapFrom(x => x.UsageMetrics));

            Mapper.CreateMap<WebSiteGetHistoricalUsageMetricsResponse.HistoricalUsageMetric, AzureHistoricalUsageMetric>()
                  .ForMember(custom => custom.Data,
                             azure => azure.MapFrom(x => x.Data));

            Mapper.CreateMap<WebSiteGetHistoricalUsageMetricsResponse.HistoricalUsageMetricData, AzureHistoricalUsageMetricData>()
                .ForMember(custom => custom.DisplayName,
                           azure => azure.MapFrom(x => x.DisplayName))
                .ForMember(custom => custom.Values,
                           azure => azure.MapFrom(x => x.Values));

            Mapper.CreateMap<WebSiteGetHistoricalUsageMetricsResponse.HistoricalUsageMetricSample, AzureHistoricalUsageMetricSample>()
                .ForMember(custom => custom.TimeCreated,
                           azure => azure.MapFrom(x => x.TimeCreated))
                .ForMember(custom => custom.Total,
                           azure => azure.MapFrom(x => x.Total));
        }
    }
}