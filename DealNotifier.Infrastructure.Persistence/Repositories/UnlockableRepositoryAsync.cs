using AutoMapper;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using DealNotifier.Core.Application.Contracts.Repositories;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Infrastructure.Persistence.Repositories
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