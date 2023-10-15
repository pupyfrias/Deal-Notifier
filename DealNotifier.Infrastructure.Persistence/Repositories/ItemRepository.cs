using AutoMapper;
using AutoMapper.QueryableExtensions;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfigurationProvider _configurationProvider;


        #region Constructor

        public ItemRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
            _dbContext = context;
            _configurationProvider = configurationProvider;
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
            return await _dbContext.Items.FirstOrDefaultAsync(item=> item.PublicId == id);
        }

        public async Task UpdateRangeAsync(IEnumerable<Item> items)
        {
             _dbContext.Items.UpdateRange(items);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateStockStatus(Expression<Func<Item, bool>> predicate, Expression<Func<SetPropertyCalls<Item>, SetPropertyCalls<Item>>> setPropertyCalls)
        {
             await _dbContext.Items.Where(predicate).ExecuteUpdateAsync(setPropertyCalls);
        }




        #endregion Constructor
    }
}