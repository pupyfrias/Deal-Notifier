using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class UnlockableRepository : GenericRepository<UnlockabledPhone, int>, IUnlockableRepository
    {
        #region Private Variable

        private readonly ApplicationDbContext _dbContext;

        #endregion Private Variable

        #region Constructor

        public UnlockableRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
            _dbContext = context;
        }

        public async Task<UnlockabledPhone?> GetByModelNumberAsync(string modelNumber)
        {
            return await _dbContext.UnlockabledPhones.FirstOrDefaultAsync(x => x.ModelNumber == modelNumber);
        }

        #endregion Constructor
    }
}