using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.OnlineStore;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
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