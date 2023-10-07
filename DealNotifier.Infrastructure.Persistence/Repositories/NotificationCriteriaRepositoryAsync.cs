using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class NotificationCriteriaRepositoryAsync : GenericRepositoryAsync<NotificationCriteria, int>, INotificationCriteriaRepositoryAsync
    {
        #region Constructor

        public NotificationCriteriaRepositoryAsync(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}