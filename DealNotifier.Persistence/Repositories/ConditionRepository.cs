using AutoMapper;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Persistence.DbContexts;

namespace Catalog.Persistence.Repositories
{
    public class ConditionRepository : GenericRepository<Condition>, IConditionRepository
    {
        #region Constructor

        public ConditionRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}