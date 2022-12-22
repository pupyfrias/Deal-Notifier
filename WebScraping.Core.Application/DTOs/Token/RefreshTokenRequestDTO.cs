namespace WebScraping.Core.Application.DTOs.Token
{
    public class RefreshTokenRequestDTO
    {
        public string Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
