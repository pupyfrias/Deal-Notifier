using AutoMapper;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Persistence.DbContexts;

namespace Catalog.Persistence.Repositories
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