using AutoMapper;
using Identity.Application.Dtos.User;
using Identity.Persistence.Entity;
using UserProto;

namespace Identity.GrpcService.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<UserCreateDto, User>().ReverseMap();
            CreateMap<UserUpdateDto, User>().ReverseMap();
            CreateMap<UserDetailDTO, User>().ReverseMap();
            CreateMap<UserListDTO, User>().ReverseMap();
            CreateMap<UserListDTO, UserList>().ReverseMap();
        }
    }

}