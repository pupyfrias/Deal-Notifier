using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class UnlockableRepositoryAsync : GenericRepositoryAsync<UnlockabledPhone, int>, IUnlockableRepositoryAsync
    {
        #region Private Variable

        private readonly ApplicationDbContext _context;

        #endregion Private Variable

        #region Constructor

        public UnlockableRepositoryAsync(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
            _context = context;
        }

        public async Task<UnlockabledPhone?> GetByModelNumberAsync(string modelNumber)
        {
            return await _context.UnlockabledPhones.FirstOrDefaultAsync(x => x.ModelNumber == modelNumber);
        }

        #endregion Constructor
    }
}