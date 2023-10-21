using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DealNotifier.Persistence.Repositories
{
    public class UnlockabledPhonePhoneUnlockToolRepository : IUnlockabledPhonePhoneUnlockToolRepository
    {
        #region Private Variable

        private readonly ApplicationDbContext _dbContext;

        #endregion Private Variable

        #region Constructor

        public UnlockabledPhonePhoneUnlockToolRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task CreateAsync(UnlockabledPhonePhoneUnlockTool entity)
        {
            await _dbContext.UnlockabledPhonePhoneUnlockTools.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateRangeAsync(IEnumerable<UnlockabledPhonePhoneUnlockTool> entities)
        {
            await _dbContext.UnlockabledPhonePhoneUnlockTools.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(UnlockabledPhonePhoneUnlockTool entity)
        {
            return await _dbContext.UnlockabledPhonePhoneUnlockTools
                .AnyAsync(x => x.PhoneUnlockToolId == entity.PhoneUnlockToolId &&
                              x.UnlockabledPhoneId == entity.UnlockabledPhoneId);
        }

        #endregion Constructor
    }
}