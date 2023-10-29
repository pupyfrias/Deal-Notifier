namespace Identity.Application.Configs
{
    public class JwtConfig
    {
        public int DurationInMinutes { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
    }
}