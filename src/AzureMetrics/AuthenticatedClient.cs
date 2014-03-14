using Microsoft.WindowsAzure;

namespace NorwegianBlue.Azure
{
    public class AuthenticatedClient
    {
        protected SubscriptionCloudCredentials SubscriptionCloudCredentials;
        protected PublishSettings PublishSettings;

        public AuthenticatedClient(PublishSettings publishSettings)
        {
            PublishSettings = publishSettings;
            SubscriptionCloudCredentials = CertificateAuthenticator.GetCredentials(publishSettings.SubscriptionId, publishSettings.ManagementCertificate);
        } 
    }
}