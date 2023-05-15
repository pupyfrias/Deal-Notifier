using AutoMapper;
using WebScraping.Core.Application.Contracts.Repositories;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.DbContexts;

namespace WebScraping.Infrastructure.Persistence.Repositories
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