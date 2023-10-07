using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Domain.Entities;
using WebApi.Controllers.V1;

namespace DealNotifier.Core.Application.Mappings
{
    public class StockStatusMapperConfig : Profile
    {
        public StockStatusMapperConfig()
        {
            CreateMap<StockStatusCreateRequest, StockStatus>().IgnoreAllSourceNullProperties();
            CreateMap<StockStatusUpdateRequest, StockStatus>().IgnoreAllSourceNullProperties();
            CreateMap<StockStatus, StockStatusResponse>().IgnoreAllSourceNullProperties();
        }
    }
}