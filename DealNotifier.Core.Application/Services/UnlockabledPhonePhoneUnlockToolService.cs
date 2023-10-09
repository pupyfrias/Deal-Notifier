using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1.PhoneUnlockTool;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Services
{
    public class UnlockabledPhonePhoneUnlockToolService : IUnlockabledPhonePhoneUnlockToolService

    {
        #region Private Variable

        private readonly IUnlockabledPhonePhoneUnlockToolRepository _repository;

        #endregion Private Variable

        public UnlockabledPhonePhoneUnlockToolService(IUnlockabledPhonePhoneUnlockToolRepository repository)
        {
            _repository = repository;
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