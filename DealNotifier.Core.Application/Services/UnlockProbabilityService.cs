using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Interfaces.Services.Items;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace DealNotifier.Core.Application.Services
{
    public class UnlockProbabilityService : GenericService<UnlockProbability>, IUnlockProbabilityService
    {
        private readonly IItemValidationService _itemValidationService;
        private readonly IUnlockVerificationService _unlockVerificationService;
        public UnlockProbabilityService(
            IUnlockProbabilityRepository repository,
            IMapper mapper, 
            IHttpContextAccessor httpContext,
            IMemoryCache cache,
            IItemValidationService itemValidationService,
            IUnlockVerificationService unlockVerificationService
            )
            : base(repository, mapper, httpContext, cache)
        {
            _unlockVerificationService = unlockVerificationService;
            _itemValidationService = itemValidationService;
        }

        public async Task SetUnlockProbabilityAsync(ItemDto itemCreate)
        {
            if (_itemValidationService.ContainsWordUnlocked(itemCreate.Name) || await _unlockVerificationService.CanBeUnlockedBasedOnUnlockabledPhoneIdAsync(itemCreate))
            {
                itemCreate.UnlockProbabilityId = (int)Enums.UnlockProbability.High;
            }
            else if (await _unlockVerificationService.CanBeUnlockedBasedOnModelNameAsync(itemCreate))
            {
                itemCreate.UnlockProbabilityId = (int)Enums.UnlockProbability.Middle;
            }
            else
            {
                itemCreate.UnlockProbabilityId = (int)Enums.UnlockProbability.Low;
            }
        }
    }
}