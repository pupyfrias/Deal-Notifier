using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebScraping.Core.Application.DTOs.Account;
using WebScraping.Core.Application.Exceptions;
using WebScraping.Core.Application.Interfaces.Services;
using WebScraping.Core.Application.Wrappers;
using WebScraping.Core.Domain.Settings;
using WebScraping.Infrastructure.Identity.Helpers;
using WebScraping.Infrastructure.Identity.Models;

namespace WebScraping.Infrastructure.Identity.Services
{
    public class AccountService: IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly ILogger _logger;
        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IOptions<JWTSettings> jwtSettings,
            ILogger logger
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                throw new ApiException($"No Accounts Registered with {request.UserName}.");
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                throw new ApiException($"Invalid Password for {request.UserName}");
            }

            if (!user.EmailConfirmed)
            {
                throw new ApiException($"Account Not Comfirmed for {user.Email}");
            }

            var jwtSecurityToken = await GenerateJWToken(user);
            var authentication = new AuthenticationResponse();
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            string Accesstoken = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
            var roles = (List<string>)await _userManager.GetRolesAsync(user);
            //var refreshToken = GenerateRefreshToken(ipAddress);
            
            authentication.UserName = user.UserName;
            authentication.Email = user.Email;
            authentication.IsVerified = user.EmailConfirmed;
            authentication.Roles = roles;
            authentication.AccessToken = Accesstoken;

            //user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            _logger.Information("User {UserName} logged in", user.UserName);

            var response = new Response<AuthenticationResponse>(authentication);
            response.Message = "Successs Authentication.";

            return response;
        }




        public async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            string ipAddress = IpHelper.GetIpAddress();

            foreach (var role in userRoles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                issuer: _jwtSettings.Issuer,
                signingCredentials: SigningCredentials
                );

            return jwtSecurityToken;

        }

        

        public string RandomTokenString()
        {
            var randomNuber = new byte[64];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNuber);
            return Convert.ToBase64String(randomNuber);
        }
    }
}
