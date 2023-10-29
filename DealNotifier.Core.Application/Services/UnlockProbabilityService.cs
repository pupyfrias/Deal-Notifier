using AutoMapper;
using Catalog.Application.Enums;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.Interfaces.Services.Items;
using Catalog.Application.ViewModels.V1.Item;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Catalog.Application.Services
{
    public class UnlockProbabilityService : GenericService<Domain.Entities.UnlockProbability>, IUnlockProbabilityService
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

        public async Task SetUnlockProbabilityAsync(ItemCreateRequest itemCreate)
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