using System.Text.Json.Serialization;

namespace WebScraping.Core.Application.Dtos.Token
{
    public class RefreshTokenResponseDto
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}