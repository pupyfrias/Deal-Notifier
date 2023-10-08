using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class UnlockabledPhonePhoneUnlockToolRepositoryAsync : IUnlockabledPhonePhoneUnlockToolRepositoryAsync
    {
        #region Private Variable

        private readonly ApplicationDbContext _context;

        #endregion Private Variable

        #region Constructor

        public UnlockabledPhonePhoneUnlockToolRepositoryAsync(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateRangeAsync(IEnumerable<UnlockabledPhonePhoneUnlockTool> entities)
        {
            await _context.UnlockablePhoneUnlockTools.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        #endregion Constructor
    }
}