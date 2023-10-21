using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Persistence.DbContexts;

namespace DealNotifier.Persistence.Repositories
{
    public class UnlockProbabilityRepository : GenericRepository<UnlockProbability>, IUnlockProbabilityRepository
    {
        #region Constructor

        public UnlockProbabilityRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}