﻿using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class BrandRepositoryAsync : GenericRepositoryAsync<Brand, int>, IBrandRepositoryAsync
    {
        #region Constructor

        public BrandRepositoryAsync(ApplicationDbContext context, IConfigurationProvider configurationProvider) : base(context, configurationProvider)
        {
        }

        #endregion Constructor
    }
}