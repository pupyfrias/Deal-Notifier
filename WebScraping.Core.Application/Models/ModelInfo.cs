
using Newtonsoft.Json;

namespace WebScraping.Core.Application.Models
{
    public class ModelInfo
    {
        public string Id { get; set; }
        public string Model { get; set; }
        public string Creditscodereader { get; set; }
        public string Creditstmo { get; set; }
        public string Creditspr { get; set; }
        public string Info { get; set; }
        public string Supportby { get; set; }
        public string Supportcarriers { get; set; }
        public string Supportversions { get; set; }
        public string Comment { get; set; }

        [JsonProperty("0")]
        public string _0 { get; set; }

        [JsonProperty("1")]
        public string _1 { get; set; }

        [JsonProperty("2")]
        public string _2 { get; set; }

        [JsonProperty("3")]
        public string _3 { get; set; }

        [JsonProperty("4")]
        public string _4 { get; set; }

        [JsonProperty("5")]
        public string _5 { get; set; }

        [JsonProperty("6")]
        public string _6 { get; set; }

        [JsonProperty("7")]
        public string _7 { get; set; }

        [JsonProperty("8")]
        public string _8 { get; set; }

        [JsonProperty("9")]
        public string _9 { get; set; }
    }

}
