namespace WebScraping.Core.Domain.Configs
{
    public class JWTConfig
    {
        public int DurationInMinutes { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Key { get; set; }
    }
}