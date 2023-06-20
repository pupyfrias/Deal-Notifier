using AutoMapper;
using DealNotifier.Core.Application.DTOs;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class BannedMapperConfig : Profile
    {
        public BannedMapperConfig()
        {
            CreateMap<Banned, BannedDto>().ReverseMap();

        }
    }
}