using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneCarrier;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class UnlockabledPhonePhoneCarrierRepository : IUnlockabledPhonePhoneCarrierRepository
    {
        #region Private Variables

        private readonly ApplicationDbContext _dbContext;

        #endregion Private Variables

        #region Constructor

        public UnlockabledPhonePhoneCarrierRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<UnlockabledPhonePhoneCarrier> CreateAsync(UnlockabledPhonePhoneCarrier entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> ExistsAsync(UnlockabledPhonePhoneCarrier entity)
        {
            return await _dbContext.UnlockabledPhonePhoneCarriers
                .AnyAsync(x => x.PhoneCarrierId == entity.PhoneCarrierId &&
                              x.UnlockabledPhoneId == entity.UnlockabledPhoneId);
        }

        public async Task CreateRangeAsync(IEnumerable<UnlockabledPhonePhoneCarrier> entities)
        {
            await _dbContext.UnlockabledPhonePhoneCarriers.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public bool Exists(UnlockabledPhonePhoneCarrier entity)
        {
            throw new NotImplementedException();
        }

        #endregion Constructor
    }
}