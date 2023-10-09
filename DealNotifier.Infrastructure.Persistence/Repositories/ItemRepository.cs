using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class ItemRepository : GenericRepository<Item, Guid>, IItemRepository
    {
        private readonly ApplicationDbContext _dbContext;


        #region Constructor

        public ItemRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
            _dbContext = context;
        }

        public async Task CreateRangeAsync(IEnumerable<Item> items)
        {
            await _dbContext.Items.AddRangeAsync(items);
            await _dbContext.SaveChangesAsync();
        }


        public async Task UpdateRangeAsync(IEnumerable<Item> items)
        {
             _dbContext.Items.UpdateRange(items);
            await _dbContext.SaveChangesAsync();
        }



        #endregion Constructor
    }
}