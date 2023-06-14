using AutoMapper;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using DealNotifier.Core.Application.Contracts.Repositories;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class UnlockablePhoneCarrierRepositoryAsync : IUnlockablePhoneCarrierRepositoryAsync
    {

        #region Private Variable
        private readonly ApplicationDbContext _context;
        #endregion Private Variable

        #region Constructor

        public UnlockablePhoneCarrierRepositoryAsync(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UnlockablePhonePhoneCarrier> CreateAsync(UnlockablePhonePhoneCarrier entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> ExistsAsync(UnlockablePhonePhoneCarrier entity)
        {
            return await _context.UnlockablePhoneCarriers
                .AnyAsync(x => x.PhoneCarrierId == entity.PhoneCarrierId &&
                              x.UnlockablePhoneId == entity.UnlockablePhoneId);
        }

        #endregion Constructor
    }
}