using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Persistence.DbContexts;

namespace DealNotifier.Persistence.Repositories
{
    public class NotificationCriteriaRepository : RepositoryBase<NotificationCriteria>, INotificationCriteriaRepository
    {
        #region Constructor

        public NotificationCriteriaRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}