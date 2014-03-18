namespace NorwegianBlue.Integration.Azure.AzureAPI
{
    public class PublishSettings
    {
        public string WebSpace { get; set; }
        public string WebSite { get; set; }
        public string SubscriptionId { get; set; }
        public string ManagementCertificate { get; set; }

        public PublishSettings()
        {
            //var configuration = YamlParser.GetConfiguration();

            //WebSpace = configuration["WebSpace"];
            //WebSite = configuration["WebSite"];
            //SubscriptionId = configuration["SubId"];
            //ManagementCertificate = configuration["ManagementCert"];
        }
    }
}