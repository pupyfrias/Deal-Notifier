using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebScraping.Core.Application.Contracts.Repositories;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.DbContexts;

namespace WebScraping.Infrastructure.Persistence.Repositories
{
    public class UnlockableRepositoryAsync : GenericRepositoryAsync<Unlockable>, IUnlockableRepositoryAsync
    {

        #region Private Variable
        private readonly ApplicationDbContext _context;
        #endregion Private Variable

        #region Constructor

        public UnlockableRepositoryAsync(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
            _context = context;
        }

        public async Task<Unlockable?> GetByModelNumberAsync(string modelNumber)
        {
            return await _context.Unlockables.FirstOrDefaultAsync(x => x.ModelNumber == modelNumber);
        }

        #endregion Constructor
    }
}