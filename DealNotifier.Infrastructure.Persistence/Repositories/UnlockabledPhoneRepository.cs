using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class UnlockabledPhoneRepository : GenericRepository<UnlockabledPhone, int>, IUnlockabledPhoneRepository
    {
        private readonly ApplicationDbContext _dbContext;

        #region Constructor

        public UnlockabledPhoneRepository(ApplicationDbContext dbContext, IConfigurationProvider configurationProvider) : base(dbContext, configurationProvider)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistsAsync(string modelName)
        {
            return await _dbContext.UnlockabledPhones.AnyAsync(unlockabledPhone => unlockabledPhone.ModelName.Equals(modelName));
        }

        #endregion Constructor
    }
}