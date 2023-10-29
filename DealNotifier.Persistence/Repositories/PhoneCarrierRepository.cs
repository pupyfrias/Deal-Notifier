using AutoMapper;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Persistence.DbContexts;

namespace Catalog.Persistence.Repositories
{
    public class PhoneCarrierRepository : GenericRepository<PhoneCarrier>, IPhoneCarrierRepository
    {
        #region Constructor

        public PhoneCarrierRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}