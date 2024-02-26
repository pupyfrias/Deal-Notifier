using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Persistence.DbContexts;
using Polly;

namespace DealNotifier.Persistence.Repositories
{
    public class BanLinkRepository : RepositoryBase<BanLink>, IBanLinkRepository
    {
        #region Constructor

        public BanLinkRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        public async Task CreateRangeAsync(IEnumerable<BanLink> banLinks)
        {
            await dbContext.BanLinks.AddRangeAsync(banLinks);
            await dbContext.SaveChangesAsync();
        }

        #endregion Constructor
    }
}