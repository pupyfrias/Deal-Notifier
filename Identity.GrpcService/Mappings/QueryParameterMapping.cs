using AutoMapper;
using Identity.Domain.Entities;
using UserProto;

namespace Identity.GrpcService.Mappings
{
    public class QueryParameterMapping: Profile
    {
        public QueryParameterMapping() 
        {
            CreateMap<QueryParametersRequest, QueryParametersDto>()
                .ForMember(dest => dest.Limit, opt => opt.Condition(src => src.Limit != 0));
                
        
        }
    }
}
