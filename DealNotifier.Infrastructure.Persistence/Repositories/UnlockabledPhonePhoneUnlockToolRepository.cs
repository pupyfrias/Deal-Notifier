using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;

namespace DealNotifier.Infrastructure.Persistence.Repositories
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

        public async Task CreateRangeAsync(IEnumerable<UnlockabledPhonePhoneUnlockTool> entities)
        {
            await _dbContext.UnlockabledPhoneUnlockTools.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        #endregion Constructor
    }
}