using WebScraping.Core.Application.Contracts.Repositories;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.DbContexts;

namespace WebScraping.Infrastructure.Persistence.Repositories
{
    public class UnlockableUnlockToolRepositoryAsync :  IUnlockableUnlockToolRepositoryAsync
    {

        #region Private Variable
        private readonly ApplicationDbContext _context;
        #endregion Private Variable

        #region Constructor

        public UnlockableUnlockToolRepositoryAsync(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UnlockableUnlockTool> CreateAsync(UnlockableUnlockTool entity)
        {
            await _context.AddAsync(entity);  
            await _context.SaveChangesAsync();
            return entity;
        }

        #endregion Constructor
    }
}