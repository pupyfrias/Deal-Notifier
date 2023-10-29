using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.UnlockProbability;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
{
    public class UnlockProbabilityMapperConfig : Profile
    {
        public UnlockProbabilityMapperConfig()
        {
            CreateMap<UnlockProbabilityCreateRequest, UnlockProbability>().IgnoreAllSourceNullProperties();
            CreateMap<UnlockProbabilityUpdateRequest, UnlockProbability>().IgnoreAllSourceNullProperties();
            CreateMap<UnlockProbability, UnlockProbabilityResponse>().IgnoreAllSourceNullProperties();
        }
    }
}