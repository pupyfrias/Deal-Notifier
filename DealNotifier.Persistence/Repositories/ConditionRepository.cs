using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Persistence.DbContexts;

namespace DealNotifier.Persistence.Repositories
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