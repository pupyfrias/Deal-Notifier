using AutoMapper;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Persistence.DbContexts;

namespace Catalog.Persistence.Repositories
{
    public class StockStatusRepository : GenericRepository<StockStatus>, IStockStatusRepository
    {
        #region Constructor

        public StockStatusRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}