using ApiGateway.Wrappers;
using UserProto;

namespace ApiGateway.Interfaces
{
    public interface IUserService
    {
        Task AddRolesAsync(Guid userId, List<string> roleList);

        Task<ApiResponse<UserCreateResponse>> CreateAsync(UserCreateRequest request);

        Task DeleteAsync(Guid id);

        Task<ApiResponse<UserExistsResponse>> ExistsAsync(UserExistsRequest request);

        Task<ApiResponse<PagedResultResponse>> GetAllAsync(QueryParametersRequest request);

        Task<ApiResponse<UserDetailsResponse>> GetByIdAsync(Guid id);

        Task<ApiResponse<UserRolesResponse>> GetRolesAsync(Guid id);

        Task RemoveRolesAsync(Guid userId, List<string> roleList);

        Task UpdateAsync(Guid id, UserUpdateRequest request);
    }
}