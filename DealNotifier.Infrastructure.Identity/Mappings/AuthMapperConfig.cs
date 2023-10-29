using AutoMapper;
using Catalog.Application.ViewModels.V1.Auth;


namespace DealNotifier.Infrastructure.Identity.Mappings
{
    public class AuthMapperConfig : Profile
    {
        public AuthMapperConfig()
        {
            CreateMap<AuthenticationResponse, AuthProto.AuthenticationResponse>()
                .ReverseMap()
                 .ForMember(dest => dest.ValidTo, opt => opt.MapFrom(src => src.ValidTo.ToDateTime()));
            CreateMap<AuthenticationRequest, AuthProto.AuthenticationRequest>().ReverseMap();
        }
    }
}