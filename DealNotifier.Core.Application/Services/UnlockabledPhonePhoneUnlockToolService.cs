using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1.PhoneUnlockTool;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneUnlockTool;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Services
{
    public class UnlockabledPhonePhoneUnlockToolService : IUnlockabledPhonePhoneUnlockToolService

    {
        #region Private Variables

        private readonly IUnlockabledPhonePhoneUnlockToolRepository _repository;
        private readonly IMapper _mapper;

        #endregion Private Variables

        public UnlockabledPhonePhoneUnlockToolService(IUnlockabledPhonePhoneUnlockToolRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task CreateAsync(UnlockabledPhonePhoneUnlockToolCreate model)
        {
            var UnlockabledPhonePhoneUnlockTool = _mapper.Map<UnlockabledPhonePhoneUnlockTool>(model);
            await _repository.CreateAsync(UnlockabledPhonePhoneUnlockTool);
        }

        public async Task CreateRangeAsync(int PhoneUnlockToolId, PhoneUnlockToolUnlockablePhoneCreateRequest request)
        {
            var unlockedPhoneList = request.UnlockablePhones.Split(',').Select(s => int.Parse(s));
            var phoneUnlockToolUnlockablePhoneList = unlockedPhoneList.Select(unlockedPhoneId =>
            {
                return new UnlockabledPhonePhoneUnlockTool
                {
                    UnlockabledPhoneId = unlockedPhoneId,
                    PhoneUnlockToolId = PhoneUnlockToolId
                };

            });

            await _repository.CreateRangeAsync(phoneUnlockToolUnlockablePhoneList);
        }
    }
}