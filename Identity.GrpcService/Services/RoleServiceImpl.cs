using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Identity.Application.Utils;
using Identity.Persistence.Contracts.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using RoleProto;

namespace Identity.GrpcService.Services
{
    [Authorize]
    public class RoleServiceImpl : RoleService.RoleServiceBase
    {
        private readonly IRoleRepositoryAsync _roleRepositoryAsync;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;

        public RoleServiceImpl(IRoleRepositoryAsync roleRepositoryAsync, IMemoryCache cache, IMapper mapper)
        {
            _roleRepositoryAsync = roleRepositoryAsync;
            _cache = cache;
            _mapper = mapper;
        }


        public override async Task<RoleResponse> GetAll(Empty request, ServerCallContext context)
        {
            string cacheKey = $"Role_List";

            if (!_cache.TryGetValue(cacheKey, out List<RoleData> mappedRoles))
            {
                var entities = await _roleRepositoryAsync.GetAllAsync();
                mappedRoles = _mapper.Map<List<RoleData>>(entities);
                _cache.Set(cacheKey, mappedRoles, TimeSpan.FromDays(7));
            }

            var eTag = CacheUtils.GenerateETag(mappedRoles);
            var response = new RoleResponse();
            response.Roles.AddRange(mappedRoles);
            response.Etag = eTag;

            return response;
        }
    }
}
