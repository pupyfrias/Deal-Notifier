﻿using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Domain.Configs;
using Microsoft.Extensions.Options;
using System;
using WorkerService.T_Unlock_WebScraping.Helpers;
using WorkerService.T_Unlock_WebScraping.Interfaces;
using ILogger = Serilog.ILogger;

namespace WorkerService.T_Unlock_WebScraping.Services
{
    public class DataSynchronizerTUnlockService : IDataSynchronizerTUnlockService
    {
        private readonly ILogger _logger;
        private readonly IFetchTUnlock _fetchTUnlock;
        private readonly IProcessPhoneTUnlock _processPhoneTUnlock;
        private readonly TUnlockUrlConfig _unlockUrlConfig;

        public DataSynchronizerTUnlockService(
            ILogger logger, 
            IFetchTUnlock fetchTUnlock, 
            IProcessPhoneTUnlock processPhoneTUnlock,
            IOptions<TUnlockUrlConfig> tUnlockUrlConfig
            
            )
        {
            _logger = logger;
            _fetchTUnlock = fetchTUnlock;
            _processPhoneTUnlock = processPhoneTUnlock;
            _unlockUrlConfig = tUnlockUrlConfig.Value;

        }

        public async Task InitializeAsync()
        {
            try
            {
                if (_unlockUrlConfig?.Paths == null)
                {
                    _logger.Warning("TUnlock URL config or paths are null. Initialization aborted.");
                    return;
                }

                foreach (string path in _unlockUrlConfig.Paths)
                {
                    try
                    {
                        var pageHtml = await _fetchTUnlock.GetPageHTMLAsync(path);

                        if (pageHtml != null)
                        {
                            var brand = BrandHelper.GetBrandByName(path);
                            await _processPhoneTUnlock.ProcessAsync(pageHtml, brand);
                        }
                        else
                        {
                            _logger.Warning($"Page HTML for path {path} is null.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"An error occurred while processing path {path}: {ex.Message}", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred during initialization: {ex.Message}", ex);
            }
        }
    }
}