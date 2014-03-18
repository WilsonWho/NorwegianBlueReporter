using Microsoft.WindowsAzure;
using NorwegianBlue.Integration.Azure.AzureAPI.Utilities;

namespace NorwegianBlue.Integration.Azure.AzureAPI
{
    public class AuthenticatedClient
    {
        protected SubscriptionCloudCredentials SubscriptionCloudCredentials;
        protected PublishSettings PublishSettings;

        public AuthenticatedClient(PublishSettings publishSettings)
        {
            Maps.Init();

            PublishSettings = publishSettings;
            SubscriptionCloudCredentials = CertificateAuthenticator.GetCredentials(publishSettings.SubscriptionId, publishSettings.ManagementCertificate);
        } 
    }
}