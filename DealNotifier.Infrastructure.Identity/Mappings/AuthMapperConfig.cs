using AutoMapper;
using DealNotifier.Core.Application.ViewModels.V1.Auth;

namespace DealNotifier.Infrastructure.Identity.Mappings
{
    public class AuthMapperConfig : Profile
    {
        public AuthMapperConfig()
        {
            CreateMap<AuthenticationResponse, AuthProto.AuthenticationResponse>().ReverseMap();
            CreateMap<AuthenticationRequest, AuthProto.AuthenticationRequest>().ReverseMap();
        }
    }
}