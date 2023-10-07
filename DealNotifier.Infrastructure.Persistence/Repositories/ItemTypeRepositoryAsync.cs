using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class ItemTypeRepositoryAsync : GenericRepositoryAsync<ItemType, int>, IItemTypeRepositoryAsync
    {
        #region Constructor

        public ItemTypeRepositoryAsync(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}