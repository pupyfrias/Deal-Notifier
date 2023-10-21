using Newtonsoft.Json;

namespace DealNotifier.Infrastructure.SamkeyDataSyncWorker.ViewModels
{
    public class PhoneDetailsResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("creditscodereader")]
        public string CreditsCodeReader { get; set; }

        [JsonProperty("creditstmo")]
        public string CreditsTmo { get; set; }

        [JsonProperty("creditspr")]
        public string CreditsPr { get; set; }

        [JsonProperty("info")]
        public string Info { get; set; }

        [JsonProperty("supportby")]
        public string SupportBy { get; set; }

        [JsonProperty("supportcarriers")]
        public string SupportCarriers { get; set; }

        [JsonProperty("supportversions")]
        public string SupportVersions { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }
    }

}
