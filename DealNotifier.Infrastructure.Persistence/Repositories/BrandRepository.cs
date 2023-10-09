using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class BrandRepository : GenericRepository<Brand, int>, IBrandRepository
    {
        #region Constructor

        public BrandRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}