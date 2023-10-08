using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1.OnlineStore;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class OnlineStoreMapperConfig : Profile
    {
        public OnlineStoreMapperConfig()
        {
            CreateMap<OnlineStoreCreateRequest, OnlineStore>().IgnoreAllSourceNullProperties();
            CreateMap<OnlineStoreUpdateRequest, OnlineStore>().IgnoreAllSourceNullProperties();
            CreateMap<OnlineStore, OnlineStoreResponse>().IgnoreAllSourceNullProperties();
        }
    }
}