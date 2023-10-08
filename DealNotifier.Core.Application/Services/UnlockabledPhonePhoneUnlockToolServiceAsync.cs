using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1.PhoneUnlockTool;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Services
{
    public class UnlockabledPhonePhoneUnlockToolServiceAsync : IUnlockabledPhonePhoneUnlockToolServiceAsync
    {
        #region Private Variable

        private readonly IUnlockabledPhonePhoneUnlockToolRepositoryAsync _repository;

        #endregion Private Variable

        public UnlockabledPhonePhoneUnlockToolServiceAsync(IUnlockabledPhonePhoneUnlockToolRepositoryAsync repository)
        {
            _repository = repository;
        }



        public async Task CreateRangeAsync(int PhoneUnlockToolId, PhoneUnlockToolUnlockablePhoneCreateRequest request)
        {
            var unlockedPhoneList = request.UnlockablePhone.Split(',')
                                               .Select(s => int.Parse(s))
                                               .ToList();
            var phoneUnlockToolUnlockablePhoneList = unlockedPhoneList.Select(unlockedPhoneId =>
            {
                return new UnlockabledPhonePhoneUnlockTool
                {
                    UnlockabledPhoneId = unlockedPhoneId,
                    PhoneUnlockToolId = PhoneUnlockToolId
                };

            }).ToList();


            await _repository.CreateRangeAsync(phoneUnlockToolUnlockablePhoneList);
        }
    }
}