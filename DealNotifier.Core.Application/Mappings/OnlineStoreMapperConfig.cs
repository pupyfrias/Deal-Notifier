using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Domain.Entities;
using WebApi.Controllers.V1;

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