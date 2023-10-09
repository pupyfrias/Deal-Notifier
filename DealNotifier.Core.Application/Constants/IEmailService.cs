﻿using DealNotifier.Core.Application.ViewModels.V1.Email;
using DealNotifier.Core.Domain.Entities;
using System.Collections.Concurrent;

namespace DealNotifier.Core.Application.Constants
{
    public interface IEmailService
    {
        Task SendAsync(EmailDto emailDTO);

        Task NotifyByEmail(ConcurrentBag<Item> items);
    }
}