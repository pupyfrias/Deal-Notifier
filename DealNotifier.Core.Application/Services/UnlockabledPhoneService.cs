using AutoMapper;
using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhone;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using WorkerService.T_Unlock_WebScraping.ViewModels;
using Brand = DealNotifier.Core.Application.Enums.Brand;

namespace DealNotifier.Core.Application.Services
{
    public class UnlockabledPhoneService : GenericService<UnlockabledPhone>, IUnlockabledPhoneService
    {

        private readonly IUnlockabledPhoneRepository _unlockedPhoneRepository;
        private readonly IUnlockabledPhonePhoneUnlockToolService _unlockabledPhonePhoneUnlockToolService;
        private readonly IUnlockabledPhonePhoneCarrierService _unlockabledPhonePhoneCarrierService;

        public UnlockabledPhoneService(
            IUnlockabledPhoneRepository unlockedPhoneRepository, 
            IMapper mapper, IHttpContextAccessor httpContext, 
            IMemoryCache cache,
            IUnlockabledPhonePhoneCarrierService unlockabledPhonePhoneCarrier,
            IUnlockabledPhonePhoneUnlockToolService unlockabledPhonePhoneUnlockToolService
            )
            : base(unlockedPhoneRepository, mapper, httpContext, cache)
        {
            _unlockedPhoneRepository = unlockedPhoneRepository;
            _unlockabledPhonePhoneCarrierService = unlockabledPhonePhoneCarrier;
            _unlockabledPhonePhoneUnlockToolService = unlockabledPhonePhoneUnlockToolService;
        }

        public async Task HandleExistingUnlockedPhoneAsync( UnlockabledPhone possibleUnlockedPhone, string carriers, UnlockTool unlockTool)
        {
            await _unlockabledPhonePhoneUnlockToolService.CreateIfNotExists(possibleUnlockedPhone.Id, (int) unlockTool);
            await _unlockabledPhonePhoneCarrierService.CreateMassiveAsync(possibleUnlockedPhone.Id, carriers);
        }

        public  async Task HandleNewUnlockedPhoneAsync(UnlockedPhoneDetailsDto unlockedPhoneDetails, Brand brand, UnlockTool unlockTool)
        {
            var newUnlockedPhone = await CreateUnlockabledPhone(unlockedPhoneDetails.ModelName, unlockedPhoneDetails.ModelNumber, (int)brand);
            await _unlockabledPhonePhoneUnlockToolService.CreateAsync(newUnlockedPhone.Id, (int)unlockTool);
            await _unlockabledPhonePhoneCarrierService.CreateMassiveAsync(newUnlockedPhone.Id, unlockedPhoneDetails.Carriers);
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