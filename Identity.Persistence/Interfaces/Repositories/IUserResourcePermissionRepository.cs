using Identity.Persistence.Repositories;

namespace Identity.Persistence.Contracts.Repositories
{
    public interface IUserResourcePermissionRepository
    {
        Task<IEnumerable<UserResourcePermissionDto>> GetResourcePermissionByUserId(string userId);
    }
}