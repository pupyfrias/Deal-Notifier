using AutoMapper;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Persistence.DbContexts;

namespace Catalog.Persistence.Repositories
{
    public class OnlineStoreRepository : GenericRepository<OnlineStore>, IOnlineStoreRepository
    {
        #region Constructor

        public OnlineStoreRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}