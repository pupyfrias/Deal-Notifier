using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneCarrier;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DealNotifier.Persistence.Repositories
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
                .AnyAsync(x => x.PhoneCarrierId == entity.PhoneCarrierId && x.UnlockabledPhoneId == entity.UnlockabledPhoneId);
        }

        public async Task CreateRangeAsync(IEnumerable<UnlockabledPhonePhoneCarrier> entities)
        {
            await _dbContext.UnlockabledPhonePhoneCarriers.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }

        public bool Exists(UnlockabledPhonePhoneCarrier entity)
        {
            return _dbContext.UnlockabledPhonePhoneCarriers
                .Any(x => x.PhoneCarrierId == entity.PhoneCarrierId && x.UnlockabledPhoneId == entity.UnlockabledPhoneId);
        }

        public UnlockabledPhonePhoneCarrier? FirstOrDefault(Expression<Func<UnlockabledPhonePhoneCarrier, bool>> predicate)
        {
            return _dbContext.UnlockabledPhonePhoneCarriers.FirstOrDefault(predicate);
        }

        public async Task<UnlockabledPhonePhoneCarrier?> FirstOrDefaultAsync(Expression<Func<UnlockabledPhonePhoneCarrier, bool>> predicate)
        {
            return await _dbContext.UnlockabledPhonePhoneCarriers.FirstOrDefaultAsync(predicate);
        }

        public bool Any(Func<UnlockabledPhonePhoneCarrier, bool> predate)
        {
           return _dbContext.UnlockabledPhonePhoneCarriers.Any(predate);
        }

        #endregion Constructor
    }
}