using AutoMapper;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using DealNotifier.Core.Application.Contracts.Repositories;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class PhoneCarrierRepositoryAsync : GenericRepositoryAsync<PhoneCarrier>, IPhoneCarrierRepositoryAsync
    {
        #region Constructor

        public PhoneCarrierRepositoryAsync(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}