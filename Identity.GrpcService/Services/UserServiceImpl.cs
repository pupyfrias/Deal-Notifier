using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Identity.Application.Exceptions;
using Identity.Application.Utils;
using Identity.Domain.Constants;
using Identity.Persistence.Contracts.Repositories;
using Identity.Persistence.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Claims;
using System.Text;
using UserProto;
using Identity.Application.Extensions;
using Identity.Domain.Entities;


namespace Identity.GrpcService.Services
{
    [Authorize]
    public class UserServiceImpl : UserService.UserServiceBase
    {
        private readonly IAuthRepositoryAsync _authRepositoryAsync;
        //private readonly IEmailServiceAsync _emailService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly IUserRepositoryAsync _userRepository;

        public UserServiceImpl(
            IUserRepositoryAsync userRepository,
            IMapper mapper,
            //IEmailServiceAsync emailService,
            IAuthRepositoryAsync authRepositoryAsync,
            IHttpContextAccessor httpContext
            )
        {
            _userRepository = userRepository;
            _mapper = mapper;
            //_emailService = emailService;
            _authRepositoryAsync = authRepositoryAsync;
            _httpContext = httpContext;
        }

        public override async Task<Empty> AddRoles(AddRolesRequest request, ServerCallContext context)
        {

            var user = await _userRepository.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new NotFoundException("User", request.UserId);
            }

            var result = await _userRepository.AddRolesAsync(user, request.Roles.ToList());
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
            return new Empty();
        }

        public override async Task<UserCreateResponse> Create(UserCreateRequest request, ServerCallContext context)
        {
            var userWithSameUserName = await _userRepository.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new BadRequestException($"Username '{request.UserName}' is already taken.");
            }
            var user = new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName
            };
            var userWithSameEmail = await _userRepository.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userRepository.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userRepository.AddToRoleAsync(user, Role.BasicUser);
                    var permissionClaim = new Claim("Permissions", "Read");
                    await _userRepository.AddClaimAsync(user, permissionClaim);

                    var verificationUri = await SendVerificationEmail(user);
                   /* await _emailService.SendAsync(new EmailDTO()
                    {
                        From = "mail@codewithmukesh.com",
                        To = user.Email,
                        Body = $"Please confirm your account by visiting this URL {verificationUri}",
                        Subject = "Confirm Registration"
                    });*/
                    return new UserCreateResponse { Result = $"User Registered. Please confirm your account by visiting this URL {verificationUri}" };
                }
                else
                {
                    throw new Exception($"{result.Errors}");
                }
            }
            else
            {
                throw new BadRequestException($"Email {request.Email} is already registered.");
            }
        }


        public override async Task<Empty> Delete(UserDeleteRequest request, ServerCallContext context)
        {
            var user = await _userRepository.FindByIdAsync(request.Id);
            if (user == null)
            {
                throw new NotFoundException("User", request.Id);
            }

            await _userRepository.DeleteAsync(user);
            return new Empty();
        }

        public override async Task<UserExistsResponse> Exists(UserExistsRequest request, ServerCallContext context)
        {
            var user = await _userRepository.FindByIdAsync(request.Id);


            return new UserExistsResponse { Result = user != null };
        }

        public override async Task<PagedResultResponse> GetAll(UserProto.QueryParametersRequest request, ServerCallContext context)
        {

                QueryParametersDto queryParametersDto = _mapper.Map<QueryParametersDto>(request);
                var userList = await _userRepository.GetAllAsync(queryParametersDto);
                var total = await _userRepository.GetTotalRecordsAsync();
                var href = _httpContext?.HttpContext?.Request.GetEncodedUrl();
                var next = UrlUtils.GetNextURL(href, request.Limit, request.Offset, total);
                var prev = UrlUtils.GetPrevURL(href, request.Limit, request.Offset, total);

                IEnumerable<UserList> mappedUserList = _mapper.Map<IEnumerable<UserList>>(userList);

                var response = new PagedResultResponse();

                response.Href = href;
                response.Prev = prev;
                response.Next = next;
                response.Offset = request.Offset;
                response.Limit = queryParametersDto.Limit;
                response.Total = total;
                response.Items.AddRange(mappedUserList);
                return response;
        }

        public override async Task<UserDetailsResponse> GetById(UserIdRequest request, ServerCallContext context)
        {
            var user = await _userRepository.FindByIdAsync(request.Id);
            if (user == null)
            {
                throw new NotFoundException("User", request.Id);
            }
            var roles = (List<string>)await _userRepository.GetRolesAsync(user);
            var mappedUser = _mapper.Map<UserDetailsResponse>(user);
            mappedUser.Roles.AddRange(roles);

            return mappedUser;
        }


        public override async Task<Empty> RemoveRoles(RemoveRolesRequest request, ServerCallContext context)
        {
            var user = await _userRepository.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new NotFoundException("User", request.UserId);
            }

            var result = await _userRepository.RemoveRolesAsync(user, request.Roles.ToList());
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            return new Empty();
        }


        public override async Task<Empty> Update(UserUpdateRequest request, ServerCallContext context)
        {

/*            var userId =  _httpContext?.HttpContext?.GetUserId();

            if (userId != Guid.Parse(request.Id))
            {
                throw new BadRequestException("Unauthorized to access to this resorce");
            }
*/
            var user = await _userRepository.FindByIdAsync(request.Id);
            if (user == null)
            {
                throw new NotFoundException("User", request.Id);
            }

            if (request.CurrentPassword != null && request.NewPassword != null && request.ConfirmPassword != null)
            {
                var result = await _userRepository.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }

            _mapper.Map(request, user);
            await _userRepository.UpdateAsync(user);
            return new Empty();
        }


        public async Task<IEnumerable<Claim>> GetPermissionsAsync(Guid id)
        {
            var user = await _userRepository.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new NotFoundException("User", id);
            }

            return await _userRepository.GetClaimsAsync(user);
        }


        public override async Task<UserRolesResponse> GetRoles(UserIdRequest request, ServerCallContext context)
        {
            var user = await _userRepository.FindByIdAsync(request.Id);
            if (user == null)
            {
                throw new NotFoundException("User", request.Id);
            }

            var userRoles = await _userRepository.GetRolesAsync(user);
            var response = new UserRolesResponse();
            response.Roles.AddRange(userRoles);
            return response;
        }


        #region Private Methods


        private async Task<string> SendVerificationEmail(User user)
        {
            var code = await _authRepositoryAsync.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var baseUrl = _httpContext?.HttpContext?.Request.GetBaseUrl();
            var url = baseUrl + "auth/confirm-email/";
            var enpointUri = new Uri(url);
            var verificationUri = QueryHelpers.AddQueryString(enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }
        #endregion Private Methods

    }
}
