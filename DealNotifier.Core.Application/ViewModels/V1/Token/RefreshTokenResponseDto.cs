using System.Text.Json.Serialization;

namespace Catalog.Application.ViewModels.V1.Token
{
    public class RefreshTokenResponseDto
    {
        public string AccessToken { get; set; }
        public int ExpiresIn { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}