using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class UnlockableUnlockToolRepositoryAsync : IUnlockableUnlockToolRepositoryAsync
    {
        #region Private Variable

        private readonly ApplicationDbContext _context;

        #endregion Private Variable

        #region Constructor

        public UnlockableUnlockToolRepositoryAsync(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UnlockabledPhoneUnlockTool> CreateAsync(UnlockabledPhoneUnlockTool entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        #endregion Constructor
    }
}