using Microsoft.WindowsAzure;
using NorwegianBlue.Azure.Utilities;

namespace NorwegianBlue.Azure
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