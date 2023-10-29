using AuthProto;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Identity.Application.Configs;
using Identity.Application.Exceptions;
using Identity.Application.Extensions;
using Identity.Application.Helpers;
using Identity.Persistence.Contracts.Repositories;
using Identity.Persistence.Entity;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ILogger = Serilog.ILogger;

namespace Identity.GrpcService.Services
{
    public class AuthServiceImpl : AuthService.AuthServiceBase
    {
        private readonly IUserRepositoryAsync _userRepositoryAsync;
        private readonly IRoleRepositoryAsync _roleRepositoryAsync;
        private readonly IAuthRepositoryAsync _authRepositoryAsync;
        private readonly IHttpContextAccessor _httpContext;
        private readonly JwtConfig _jwtConfig;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly RSA _rsa;
        private readonly IUserResourcePermissionRepository _userResourcePermissionRepository;

        public AuthServiceImpl(
            IAuthRepositoryAsync authRepositoryAsync,
            IConfiguration configuration,
            IHttpContextAccessor httpContext,
            ILogger logger,
            IOptions<JwtConfig> jwtConfig,
            IRoleRepositoryAsync roleRepositoryAsync,
            IUserRepositoryAsync userRepositoryAsync,
            RSA rSA,
            IUserResourcePermissionRepository userResourcePermissionRepository
            )
        {
            _authRepositoryAsync = authRepositoryAsync;
            _configuration = configuration;
            _httpContext = httpContext;
            _jwtConfig = jwtConfig.Value;
            _logger = logger;
            _roleRepositoryAsync = roleRepositoryAsync;
            _userRepositoryAsync = userRepositoryAsync;
            _rsa = rSA;
            _logger = logger;
            _userResourcePermissionRepository = userResourcePermissionRepository;
        }

        public override async Task<PublicKeyResponse> PublicKey(Empty request, ServerCallContext context)
        {
            var publicKey = _rsa.ExportSubjectPublicKeyInfo();
            var publicKeyString = Convert.ToBase64String(publicKey);
            var response = new PublicKeyResponse
            {
                PublicKey = publicKeyString
            };

            return response;
        }

        public override async Task<AuthenticationResponse> login(AuthenticationRequest request, ServerCallContext context)
        {

            User? user = await _userRepositoryAsync.FindByNameAsync(request.UserName);
            if (user == null)
            {
                user = await _userRepositoryAsync.FindByEmailAsync(request.UserName);
                if (user == null)
                {
                    throw new NotFoundException($"No Accounts Registered with {request.UserName}.");
                }

            }
            var result = await _authRepositoryAsync.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new NotFoundException( $"No Accounts Registered with {request.UserName}.");
            }
            if (!user.EmailConfirmed)
            {
                throw new NotFoundException($"Account Not Confirmed for '{request.UserName}'.");
            }

            var response = new AuthenticationResponse();
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            var refreshToken = GenerateRefreshToken(string.Empty);
            var rolesList = jwtSecurityToken.Claims.Where(c=> c.Type == "Roles").Select(claim=> claim.Value).ToList();
            var resourcePermissionList = jwtSecurityToken.Claims.Where(c=> c.Type == "ResourcePermission")
                .Select(claim=> claim.Value).ToList();

            response.Roles.AddRange(rolesList);
            response.ResourcePermissions.AddRange(resourcePermissionList);


            response.Id = user.Id;
            response.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.IsVerified = user.EmailConfirmed;
            response.RefreshToken = refreshToken.Token;
            response.ValidTo = Timestamp.FromDateTime(jwtSecurityToken.ValidTo);
            return response;



        }

        public override async Task<ConfirmEmailResponse> ConfirmEmail(ConfirmEmailRequest request, ServerCallContext context)
        {
            var user = await _userRepositoryAsync.FindByIdAsync(request.UserId);
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            var result = await _authRepositoryAsync.ConfirmEmailAsync(user, code);
            var baseUrl = _httpContext?.HttpContext?.Request.GetBaseUrl();
            if (result.Succeeded)
            {
                return new ConfirmEmailResponse { Result = $"Account Confirmed for {user.Email}. You can now use the {baseUrl}auth/login endpoint." };
            }
            else
            {
                throw new Exception($"An error occured while confirming {user.Email}.");
            }
        }

        public override async Task<Empty> ForgotPassword(ForgotPasswordRequest request, ServerCallContext context)
        {
            var account = await _userRepositoryAsync.FindByEmailAsync(request.Email);

            // always return ok response to prevent email enumeration
            if (account == null) return new Empty();

            var code = await _authRepositoryAsync.GeneratePasswordResetTokenAsync(account);
            var url = _httpContext?.HttpContext?.Request.GetEncodedUrl();
            var resetPasswordUrl = url?.Replace("forgot", "reset");
            /*var emailRequest = new EmailDTO()
            {
                Body = $"You reset token is  {code}\n Enter to this endpoint {resetPasswordUrl} to reset your password",
                To = request.Email,
                Subject = "Reset Password",
            };
            await _emailServiceAsync.SendAsync(emailRequest);*/
            return new Empty();
        }

        public override async Task<Empty> ResetPassword(ResetPasswordRequest request, ServerCallContext context)
        {
            var account = await _userRepositoryAsync.FindByEmailAsync(request.Email);

            // always return ok response to prevent email enumeration
            if (account == null) return new Empty();

            var code = await _authRepositoryAsync.GeneratePasswordResetTokenAsync(account);
            var url = _httpContext?.HttpContext?.Request.GetEncodedUrl();
            var resetPasswordUrl = url?.Replace("forgot", "reset");
            /* var emailRequest = new EmailDTO()
             {
                 Body = $"You reset token is  {code}\n Enter to this endpoint {resetPasswordUrl} to reset your password",
                 To = request.Email,
                 Subject = "Reset Password",
             };
             await _emailServiceAsync.SendAsync(emailRequest);*/

            return new Empty();
        }

        #region Private Methods
        private async Task<JwtSecurityToken> GenerateJWToken(User user)
        {
            var claimList = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim("uid", user.Id),

            };


            var roleList = await _userRepositoryAsync.GetRolesAsync(user);
            var resourcePermissionList = await _userResourcePermissionRepository.GetResourcePermissionByUserId(user.Id);
            var userClaimList = await _userRepositoryAsync.GetClaimsAsync(user);
            claimList.AddRange(userClaimList);

            foreach (var role in roleList)
            {
                claimList.Add(new Claim("Roles", role));
            }
            foreach (var rp in resourcePermissionList)
            {
                claimList.Add(new Claim("ResourcePermission", $"{rp.ResourceName}_{rp.PermissionName}"));
            }


            var signingCredentials = new SigningCredentials(new RsaSecurityKey(_rsa), SecurityAlgorithms.RsaSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claimList,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfig.DurationInMinutes),
                signingCredentials: signingCredentials
                );

            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(7)),
                Created = Timestamp.FromDateTime(DateTime.UtcNow),
                CreatedByIp = ipAddress
            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        #endregion Private Methods
    }
}
