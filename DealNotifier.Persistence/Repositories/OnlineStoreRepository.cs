﻿using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Persistence.DbContexts;

namespace DealNotifier.Persistence.Repositories
{
    public class OnlineStoreRepository : RepositoryBase<OnlineStore>, IOnlineStoreRepository
    {
        #region Constructor

        public OnlineStoreRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}