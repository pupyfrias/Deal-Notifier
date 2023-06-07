using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebScraping.Core.Application.Contracts.Repositories;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.DbContexts;

namespace WebScraping.Infrastructure.Persistence.Repositories
{
    public class UnlockableRepositoryAsync : GenericRepositoryAsync<UnlockablePhone>, IUnlockableRepositoryAsync
    {

        #region Private Variable
        private readonly ApplicationDbContext _context;
        #endregion Private Variable

        #region Constructor

        public UnlockableRepositoryAsync(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
            _context = context;
        }

        public async Task<UnlockablePhone?> GetByModelNumberAsync(string modelNumber)
        {
            return await _context.UnlockablePhones.FirstOrDefaultAsync(x => x.ModelNumber == modelNumber);
        }

        #endregion Constructor
    }
}