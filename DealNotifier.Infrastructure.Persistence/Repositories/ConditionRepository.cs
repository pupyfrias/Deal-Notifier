using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class ConditionRepository : GenericRepository<Condition, int>, IConditionRepository
    {
        #region Constructor

        public ConditionRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}