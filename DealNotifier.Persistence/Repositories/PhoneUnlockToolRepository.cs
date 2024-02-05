using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Persistence.DbContexts;

namespace DealNotifier.Persistence.Repositories
{
    public class PhoneUnlockToolRepository : RepositoryBase<PhoneUnlockTool>, IPhoneUnlockToolRepository
    {
        #region Constructor

        public PhoneUnlockToolRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}