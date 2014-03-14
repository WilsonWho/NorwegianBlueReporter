using System;
using System.Collections.Generic;
using LaserYaml;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.WebSites;
using Microsoft.WindowsAzure.Management.WebSites.Models;

namespace NorwegianBlue.Azure
{
    class Program
    {
        static void Main(string[] args)
        {
            YamlParser.SetPublicConfigurationFile(@"../../../private.yaml");

            var publishSettings = new PublishSettings();
            var subscriptionCreds = CertificateAuthenticator.GetCredentials(publishSettings.SubscriptionId, publishSettings.ManagementCertificate);
            var websiteManagementClient = CloudContext.Clients.CreateWebSiteManagementClient(subscriptionCreds);

            var webSiteGetHistoricalUsageMetricsParameters = new WebSiteGetHistoricalUsageMetricsParameters
                {
                    MetricNames = new List<string> { "CpuTime", "AverageMemoryWorkingSet", "MemoryWorkingSet" },
                    StartTime = new DateTime(2014, 2, 24, 16, 13, 0),
                    EndTime = new DateTime(2014, 2, 24, 16, 40, 0),
                };

            var response = websiteManagementClient.WebSites.GetHistoricalUsageMetrics("westuswebspace", "iq-azgptpproductlibrary1", webSiteGetHistoricalUsageMetricsParameters);
        }
    }
}
