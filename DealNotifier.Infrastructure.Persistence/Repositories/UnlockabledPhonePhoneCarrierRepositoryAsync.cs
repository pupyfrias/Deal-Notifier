using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class UnlockabledPhonePhoneCarrierRepositoryAsync : IUnlockabledPhonePhoneCarrierRepositoryAsync
    {
        #region Private Variable

        private readonly ApplicationDbContext _context;

        #endregion Private Variable

        #region Constructor

        public UnlockabledPhonePhoneCarrierRepositoryAsync(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UnlockabledPhonePhoneCarrier> CreateAsync(UnlockabledPhonePhoneCarrier entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> ExistsAsync(UnlockabledPhonePhoneCarrier entity)
        {
            return await _context.UnlockabledPhonePhoneCarriers
                .AnyAsync(x => x.PhoneCarrierId == entity.PhoneCarrierId &&
                              x.UnlockabledPhoneId == entity.UnlockabledPhoneId);
        }

        public async Task CreateRangeAsync(IEnumerable<UnlockabledPhonePhoneCarrier> entities)
        {
            await _context.UnlockabledPhonePhoneCarriers.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        #endregion Constructor
    }
}