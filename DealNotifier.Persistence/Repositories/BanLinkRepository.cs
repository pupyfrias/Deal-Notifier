using AutoMapper;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Persistence.DbContexts;

namespace Catalog.Persistence.Repositories
{
    public class BanLinkRepository : GenericRepository<BanLink>, IBanLinkRepository
    {
        #region Constructor

        public BanLinkRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}