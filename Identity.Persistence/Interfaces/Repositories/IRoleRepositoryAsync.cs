using System.Security.Claims;
using Identity.Application.Dtos.Role;

namespace Identity.Persistence.Contracts.Repositories
{
    public interface IRoleRepositoryAsync
    {
        Task<IList<RoleListDTO>> GetAllAsync();

        Task<IList<Claim>> GetClaims(string roleName);
    }
}