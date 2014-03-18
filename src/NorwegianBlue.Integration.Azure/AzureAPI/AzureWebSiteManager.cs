using AutoMapper;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.WebSites;
using Microsoft.WindowsAzure.Management.WebSites.Models;
using NorwegianBlue.Integration.Azure.AzureAPI.DTOs.WebSiteGetHistoricalUsageMetricsResponse;

namespace NorwegianBlue.Integration.Azure.AzureAPI
{
    public class AzureWebSiteManager : AuthenticatedClient
    {
        private readonly WebSiteManagementClient _webSiteManagementClient;

        public AzureWebSiteManager(PublishSettings publishSettings) : base(publishSettings)
        {
            _webSiteManagementClient = CloudContext.Clients.CreateWebSiteManagementClient(SubscriptionCloudCredentials);
        }

        public AzureGetHistoricalUsageMetricsResponse GetHistoricalUsageMetrics(AzureGetHistoricalUsageMetricsRequest azureGetHistoricalUsageMetricsDto)
        {
            var webSiteGetHistoricalUsageMetricsParameters = new WebSiteGetHistoricalUsageMetricsParameters
                {
                    StartTime = azureGetHistoricalUsageMetricsDto.StartTime,
                    EndTime = azureGetHistoricalUsageMetricsDto.EndTime,
                    MetricNames = azureGetHistoricalUsageMetricsDto.MetricNames
                };

            var response = _webSiteManagementClient.WebSites.GetHistoricalUsageMetrics(PublishSettings.WebSpace, PublishSettings.WebSite, webSiteGetHistoricalUsageMetricsParameters);

            return Mapper.Map<WebSiteGetHistoricalUsageMetricsResponse, AzureGetHistoricalUsageMetricsResponse>(response);
        }
    }
}