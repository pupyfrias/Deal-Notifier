﻿namespace DealNotifier.Core.Application.DTOs.Token
{
    public class RefreshTokenRequestDto
    {
        public string AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}