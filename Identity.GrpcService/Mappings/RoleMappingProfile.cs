using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Identity.Application.Dtos.Role;

namespace Identity.GrpcService.Mappings
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {            CreateMap<RoleListDTO, IdentityRole>().ReverseMap();
        }
    }
}