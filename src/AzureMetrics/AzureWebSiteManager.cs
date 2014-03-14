using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.WebSites;
using Microsoft.WindowsAzure.Management.WebSites.Models;
using NorwegianBlue.DataModels.Azure;
using NorwegianBlue.DataModels.Utilities;

namespace NorwegianBlue.Azure
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

            return DataMapper.Map<WebSiteGetHistoricalUsageMetricsResponse, AzureGetHistoricalUsageMetricsResponse>(response);
        }
    }
}