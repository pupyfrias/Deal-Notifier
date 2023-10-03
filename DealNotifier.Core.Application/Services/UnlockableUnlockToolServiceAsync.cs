﻿using DealNotifier.Core.Application.Contracts.Repositories;
using DealNotifier.Core.Application.Contracts.Services;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Services
{
    public class UnlockableUnlockToolServiceAsync : IUnlockableUnlockToolServiceAsync
    {
        #region Private Variable
        private readonly IUnlockableUnlockToolRepositoryAsync _repository;
        #endregion Private Variable

        public UnlockableUnlockToolServiceAsync(IUnlockableUnlockToolRepositoryAsync repository)
        {
            _repository = repository;
        }

        public async Task<UnlockablePhoneUnlockTool> CreateAsync(UnlockablePhoneUnlockTool entity)
        {
            return await _repository.CreateAsync(entity);
        }
    }
}