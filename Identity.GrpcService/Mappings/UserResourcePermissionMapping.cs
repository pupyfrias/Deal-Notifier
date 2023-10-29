using AutoMapper;
using Identity.Persistence.Entity;
using Identity.Persistence.Repositories;

namespace Identity.GrpcService.Mappings
{
    public class UserResourcePermissionMapping: Profile
    {
        public UserResourcePermissionMapping()
        {
            CreateMap<UserResourcePermissionDto, UserResourcePermission>().ReverseMap();
        }
    }
}
