using AutoMapper;
using Catalog.Application.Interfaces.Services;
using Catalog.Domain.Entities;
using Catalog.Persistence.DbContexts;

namespace Catalog.Persistence.Repositories
{
    public class NotificationCriteriaRepository : GenericRepository<NotificationCriteria>, INotificationCriteriaRepository
    {
        #region Constructor

        public NotificationCriteriaRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}