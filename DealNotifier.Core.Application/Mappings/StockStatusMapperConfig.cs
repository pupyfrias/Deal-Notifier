using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.StockStatus;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
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