using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1.StockStatus;
using DealNotifier.Core.Domain.Entities;

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