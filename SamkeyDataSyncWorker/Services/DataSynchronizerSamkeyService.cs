using SamkeyDataSyncWorker.Interfaces;
using ILogger = Serilog.ILogger;

namespace SamkeyDataSyncWorker.Services
{
    public class DataSynchronizerSamkeyService : IDataSynchronizerSamkeyService
    {
        private readonly ILogger _logger;
        private readonly IFetchSamkey _fetchSamkey;
        private readonly IProcessPhoneSamkey _processPhoneSamkey;

        public DataSynchronizerSamkeyService(ILogger logger, IFetchSamkey fetchSamkey, IProcessPhoneSamkey processPhoneSamkey)
        {
            _logger = logger;
            _fetchSamkey = fetchSamkey;
            _processPhoneSamkey = processPhoneSamkey;

        }

        public async Task InitializeAsync()
        {
            try
            {
                var phoneModelList = await _fetchSamkey.GetPhoneModelsAsync();

                if (phoneModelList != null)
                {
                    _logger.Information($"Processing {phoneModelList.Count} Phones");
                    await _processPhoneSamkey.ProcessAsync(phoneModelList);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }
    }
}