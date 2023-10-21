using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Persistence.DbContexts;

namespace DealNotifier.Persistence.Repositories
{
    public class BanKeywordRepository : GenericRepository<BanKeyword>, IBanKeywordRepository
    {
        #region Constructor

        public BanKeywordRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}