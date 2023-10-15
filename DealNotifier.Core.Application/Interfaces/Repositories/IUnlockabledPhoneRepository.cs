﻿using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IUnlockabledPhoneRepository : IGenericRepository<UnlockabledPhone>
    {
        Task<bool> ExistsAsync(string modelName);
    }
}