using ApiGateway.Exceptions;
using ApiGateway.Interfaces;
using ApiGateway.Wrappers;
using Grpc.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using UserProto;

namespace ApiGateway.Services
{
    public class UserService: IUserService
    {
        private readonly UserProto.UserService.UserServiceClient _userServiceClient;
        public UserService(UserProto.UserService.UserServiceClient userServiceClient)
        {

            _userServiceClient = userServiceClient;

        }

        public async Task AddRolesAsync(Guid userId, List<string> roleList)
        {

            AddRolesRequest request = new();
            request.Roles.AddRange(roleList);
            request.UserId = userId.ToString();
           await _userServiceClient.AddRolesAsync(request);
        }



        public async Task RemoveRolesAsync(Guid userId, List<string> roleList)
        {

            RemoveRolesRequest request = new();
            request.Roles.AddRange(roleList);
            request.UserId = userId.ToString();
            await _userServiceClient.RemoveRolesAsync(request);
        }

        public async Task<ApiResponse<UserCreateResponse>> CreateAsync(UserCreateRequest request)
        {
            var response = await _userServiceClient.CreateAsync(request);
            return new ApiResponse<UserCreateResponse>(response);   
        }

        public async Task DeleteAsync(Guid id)
        {
            UserDeleteRequest request = new() { Id = id.ToString()};
            await _userServiceClient.DeleteAsync(request);
        }

        public async Task<ApiResponse<UserExistsResponse>> ExistsAsync(UserExistsRequest request)
        {
            var response = await _userServiceClient.ExistsAsync(request);
            return new ApiResponse<UserExistsResponse>(response);
        }

        public async Task<ApiResponse<PagedResultResponse>> GetAllAsync(QueryParametersRequest request)
        {
            var response = await _userServiceClient.GetAllAsync(request);
            return new ApiResponse<PagedResultResponse>(response);
        }

        public async Task<ApiResponse<UserDetailsResponse>> GetByIdAsync(Guid id)
        {
            UserIdRequest request = new() { Id = id.ToString()};
            var response = await _userServiceClient.GetByIdAsync(request);
            return new ApiResponse<UserDetailsResponse>(response);
        }


        public async Task<ApiResponse<UserRolesResponse>> GetRolesAsync(Guid id)
        {
            UserIdRequest request = new() { Id = id.ToString() };
            var response = await _userServiceClient.GetRolesAsync(request);
            return new ApiResponse<UserRolesResponse>(response);
        }

        public async Task UpdateAsync(Guid id, UserUpdateRequest request)
        {
            if (request.Id != id.ToString()) 
                throw new BadRequestException($"The ID passed in the path ({id}) does not match the ID passed in the request body ({request.Id}).");
            
            await _userServiceClient.UpdateAsync(request);
        }
    }
}
