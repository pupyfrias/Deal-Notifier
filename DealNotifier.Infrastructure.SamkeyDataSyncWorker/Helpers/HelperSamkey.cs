using Catalog.Application.ViewModels.V1.UnlockabledPhone;

namespace DealNotifier.Infrastructure.SamkeyDataSyncWorker.Helpers
{
    public static class HelperSamkey
    {
        public static UnlockedPhoneDetailsDto? MapStringToUnlockedPhoneDetails(string phoneModel)
        {
            var splitPhone = phoneModel.Split(" | ");
            if (splitPhone.Length < 2) return null;

            return new UnlockedPhoneDetailsDto { ModelNumber = splitPhone[0], ModelName = splitPhone[1] };
        }

    }
}
