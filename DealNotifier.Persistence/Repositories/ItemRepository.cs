using AutoMapper;
using AutoMapper.QueryableExtensions;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Persistence.DbContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Linq;
using System.Linq.Expressions;

namespace DealNotifier.Persistence.Repositories
{
    public class ItemRepository : RepositoryBase<Item>, IItemRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger _logger;
        private readonly IConfigurationProvider _configurationProvider;


        #region Constructor

        public ItemRepository(
            ApplicationDbContext context,
            IConfigurationProvider configurationProvider,
            ILogger logger
            )
            : base(context, configurationProvider)
        {
            _dbContext = context;
            _configurationProvider = configurationProvider;
            _logger = logger;
        }


        public async Task DeleteRangeAsync(Expression<Func<Item, bool>> predicate)
        {
            await _dbContext.Items.Where(predicate).ExecuteDeleteAsync();
        }

        public async Task CreateRangeAsync(IEnumerable<Item> items)
        {
            await _dbContext.Items.AddRangeAsync(items);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TDestination?> GetByPublicIdProjected<TDestination>(Guid id)
        {
            var query = _dbContext.Items.AsQueryable();
            return await query.Where(x => x.PublicId.Equals(id))
                              .ProjectTo<TDestination>(_configurationProvider)
                              .FirstOrDefaultAsync();
        }

        public async Task<Item?> GetByPublicIdAsync(Guid id)
        {
            return await _dbContext.Items.FirstOrDefaultAsync(item => item.PublicId == id);
        }

        public async Task UpdateRangeAsync(IEnumerable<Item> items)
        {
            _dbContext.Items.UpdateRange(items);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateStockStatusAsync(string query, SqlParameter idListString, SqlParameter onlineStoreId,
            SqlParameter outputResult, SqlParameter errorMessage)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(query, idListString, onlineStoreId, outputResult, errorMessage);
            var successful = (bool)outputResult.Value;

            if (!successful) _logger.Error($"An error occurred while updating the item's stock status. Error {errorMessage.Value}");

        }


        #endregion Constructor
    }
}