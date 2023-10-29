using AutoMapper;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.Repositories
{
    public class UnlockabledPhoneRepository : GenericRepository<UnlockabledPhone>, IUnlockabledPhoneRepository
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