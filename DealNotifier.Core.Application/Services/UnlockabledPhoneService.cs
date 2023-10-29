using AutoMapper;
using Catalog.Application.Constants;
using Catalog.Application.Enums;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.ViewModels.V1.Item;
using Catalog.Application.ViewModels.V1.UnlockabledPhone;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using System.Text.RegularExpressions;
using Brand = Catalog.Application.Enums.Brand;

namespace Catalog.Application.Services
{
    public class UnlockabledPhoneService : GenericService<UnlockabledPhone>, IUnlockabledPhoneService
    {

        private readonly IUnlockabledPhoneRepository _unlockabledPhoneRepository;
        private readonly IUnlockabledPhonePhoneUnlockToolService _unlockabledPhonePhoneUnlockToolService;
        private readonly IUnlockabledPhonePhoneCarrierService _unlockabledPhonePhoneCarrierService;
        private readonly ILogger _logger;

        public UnlockabledPhoneService(
            IUnlockabledPhoneRepository unlockedPhoneRepository,
            IMapper mapper, IHttpContextAccessor httpContext,
            IMemoryCache cache,
            IUnlockabledPhonePhoneCarrierService unlockabledPhonePhoneCarrier,
            IUnlockabledPhonePhoneUnlockToolService unlockabledPhonePhoneUnlockToolService,
            ILogger logger
            )
            : base(unlockedPhoneRepository, mapper, httpContext, cache)
        {
            _unlockabledPhoneRepository = unlockedPhoneRepository;
            _unlockabledPhonePhoneCarrierService = unlockabledPhonePhoneCarrier;
            _unlockabledPhonePhoneUnlockToolService = unlockabledPhonePhoneUnlockToolService;
            _logger = logger;
        }

        public async Task HandleExistingUnlockedPhoneAsync(UnlockabledPhone possibleUnlockedPhone, string carriers, UnlockTool unlockTool)
        {
            await _unlockabledPhonePhoneUnlockToolService.CreateIfNotExists(possibleUnlockedPhone.Id, (int)unlockTool);
            await _unlockabledPhonePhoneCarrierService.CreateMassiveAsync(possibleUnlockedPhone.Id, carriers);
        }

        public async Task HandleNewUnlockedPhoneAsync(UnlockedPhoneDetailsDto unlockedPhoneDetails, Brand brand, UnlockTool unlockTool)
        {
            var newUnlockedPhone = await CreateUnlockabledPhone(unlockedPhoneDetails.ModelName, unlockedPhoneDetails.ModelNumber, (int)brand);
            await _unlockabledPhonePhoneUnlockToolService.CreateAsync(newUnlockedPhone.Id, (int)unlockTool);
            await _unlockabledPhonePhoneCarrierService.CreateMassiveAsync(newUnlockedPhone.Id, unlockedPhoneDetails.Carriers);
        }



        public async Task TryAssignUnlockabledPhoneIdAsync(ItemCreateRequest itemCreate)
        {
            var possibleModelNumber = Regex.Match(itemCreate.Name, RegExPattern.ModelNumber, RegexOptions.IgnoreCase).Value;

            if (!string.IsNullOrEmpty(possibleModelNumber))
            {
                var possibleUnlockabledPhone = await _unlockabledPhoneRepository.FirstOrDefaultAsync(element => element.ModelNumber.Contains(possibleModelNumber));

                if (possibleUnlockabledPhone != null)
                {
                    itemCreate.UnlockabledPhoneId = possibleUnlockabledPhone.Id;
                }
                else
                {
                    _logger.Warning($"Unlockabled Phone with ModelNumber [{possibleModelNumber}] wasn't found\n Title: {itemCreate.Name}");
                }
            }
        }


        private async Task<UnlockabledPhoneResponse> CreateUnlockabledPhone(string modelName, string modelNumber, int brandId)
        {
            var unlockabledPhoneCreate = new UnlockabledPhoneCreateRequest
            {
                BrandId = brandId,
                ModelName = modelName,
                ModelNumber = modelNumber
            };

            return await CreateAsync<UnlockabledPhoneCreateRequest, UnlockabledPhoneResponse>(unlockabledPhoneCreate);
        }

    }
}