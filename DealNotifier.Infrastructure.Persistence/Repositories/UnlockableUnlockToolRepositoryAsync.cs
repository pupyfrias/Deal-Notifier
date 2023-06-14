using DealNotifier.Infrastructure.Persistence.DbContexts;
using DealNotifier.Core.Application.Contracts.Repositories;
using DealNotifier.Core.Domain.Entities;

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

        public async Task<UnlockablePhoneUnlockTool> CreateAsync(UnlockablePhoneUnlockTool entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        #endregion Constructor
    }
}