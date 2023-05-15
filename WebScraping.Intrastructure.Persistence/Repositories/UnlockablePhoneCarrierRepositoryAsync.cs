using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebScraping.Core.Application.Contracts.Repositories;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.DbContexts;

namespace WebScraping.Infrastructure.Persistence.Repositories
{
    public class UnlockablePhoneCarrierRepositoryAsync :  IUnlockablePhoneCarrierRepositoryAsync
    {

        #region Private Variable
        private readonly ApplicationDbContext _context;
        #endregion Private Variable

        #region Constructor

        public UnlockablePhoneCarrierRepositoryAsync(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UnlockablePhoneCarrier> CreateAsync(UnlockablePhoneCarrier entity)
        {
            await _context.AddAsync(entity);  
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> ExistsAsync(UnlockablePhoneCarrier entity)
        {
            return  await _context.UnlockablePhoneCarriers
                .AnyAsync(x=> x.PhoneCarrierId == entity.PhoneCarrierId &&
                              x.UnlockableId == entity.UnlockableId);
        }

        #endregion Constructor
    }
}