using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class PhoneUnlockToolRepositoryAsync : GenericRepositoryAsync<PhoneUnlockTool, int>, IPhoneUnlockToolRepositoryAsync
    {
        #region Constructor

        public PhoneUnlockToolRepositoryAsync(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}