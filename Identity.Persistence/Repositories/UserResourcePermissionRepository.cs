using AutoMapper;
using AutoMapper.QueryableExtensions;
using Identity.Persistence.Contracts.Repositories;
using Identity.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Identity.Persistence.Repositories
{
    public class UserResourcePermissionRepository: IUserResourcePermissionRepository
    {
        private readonly IdentityContext _identityContext;
        private readonly IConfigurationProvider _configurationProvider;
        public UserResourcePermissionRepository(IdentityContext identityContext, IConfigurationProvider configurationProvider)
        {
            _identityContext = identityContext;
            _configurationProvider = configurationProvider;
        }

        public async Task<IEnumerable<UserResourcePermissionDto>> GetResourcePermissionByUserId(string userId) 
        {

            return await _identityContext.UserResourcePermissions
                .Where(x => x.UserId == userId)
                .ProjectTo<UserResourcePermissionDto>(_configurationProvider)
                .ToArrayAsync();
        
        }
    }
}
