using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Persistence.DbContexts;

namespace DealNotifier.Persistence.Repositories
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