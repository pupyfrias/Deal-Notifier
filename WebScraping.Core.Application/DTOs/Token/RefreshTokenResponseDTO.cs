using System.Text.Json.Serialization;

namespace WebScraping.Core.Application.DTOs.Token
{
    public class RefreshTokenResponseDTO
    {
        public string AccessToken { get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
