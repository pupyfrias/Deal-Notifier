using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1.UnlockProbability;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
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