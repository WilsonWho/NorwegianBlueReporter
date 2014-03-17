using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.WindowsAzure;

namespace NorwegianBlue.Azure
{
    internal class CertificateAuthenticator
    {
         internal static SubscriptionCloudCredentials GetCredentials(string subscriptionId, string base64EncodedCert)
         {
             return new CertificateCloudCredentials(subscriptionId,
                                                    new X509Certificate2(Convert.FromBase64String(base64EncodedCert)));
         }
    }
}