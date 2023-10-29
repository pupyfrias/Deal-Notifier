using AutoMapper;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Persistence.DbContexts;

namespace Catalog.Persistence.Repositories
{
    public class PhoneUnlockToolRepository : GenericRepository<PhoneUnlockTool>, IPhoneUnlockToolRepository
    {
        #region Constructor

        public PhoneUnlockToolRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}