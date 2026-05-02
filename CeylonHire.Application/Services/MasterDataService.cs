using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;
using CeylonHire.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeylonHire.Application.Services
{
    public class MasterDataService : IMasterDataService
    {
        private readonly IMasterDataRepository _masterDataRepository;
        private readonly IMemoryCache _cache;
        private readonly ILogger<MasterDataService> _logger;

        private const string CacheKey = "JOB-MASTER-DATA";
        public MasterDataService(IMasterDataRepository masterDataRepository, IMemoryCache cache, ILogger<MasterDataService> logger)
        {
            _masterDataRepository = masterDataRepository;
            _cache = cache;
            _logger = logger;
        }

        public async Task<JobMasterDataResult?> GetJobMasterDataAsync()
        {
            if(_cache.TryGetValue(CacheKey, out JobMasterDataResult? cachedData))
            {
                _logger.LogInformation("Loaded from cache.");
                return cachedData;
            }

            _logger.LogInformation("Loaded from database.");
            var data = await _masterDataRepository.GetJobMasterDataAsync();
            _cache.Set(CacheKey, data, TimeSpan.FromHours(6));
            return data;
        }
    }
}
