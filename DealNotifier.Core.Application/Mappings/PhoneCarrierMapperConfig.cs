﻿using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class PhoneCarrierMapperConfig : Profile
    {
        public PhoneCarrierMapperConfig()
        {
            CreateMap<PhoneCarrierDto, PhoneCarrier>().ReverseMap();
            CreateMap<PhoneCarrierCreateRequest, PhoneCarrier>().IgnoreAllSourceNullProperties();
            CreateMap<PhoneCarrierUpdateRequest, PhoneCarrier>().IgnoreAllSourceNullProperties();
            CreateMap<PhoneCarrier, PhoneCarrierResponse>().IgnoreAllSourceNullProperties();
        }
    }
}