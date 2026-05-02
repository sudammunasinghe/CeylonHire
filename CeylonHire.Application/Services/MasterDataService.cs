using CeylonHire.Application.Interfaces.IRepositories;
using CeylonHire.Application.Interfaces.IServices;
using CeylonHire.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
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
            if (_cache.TryGetValue(CacheKey, out JobMasterDataResult? cachedData))
            {
                _logger.LogInformation("Loaded from cache.");
                return cachedData;
            }

            _logger.LogInformation("Loaded from database.");
            var data = await _masterDataRepository.GetJobMasterDataAsync();
            _cache.Set(CacheKey, data, TimeSpan.FromHours(6));
            return data;

            //var cacheData = await _cache.GetStringAsync(CacheKey);
            //if(cacheData == null)
            //{
            //    var data = await _masterDataRepository.GetJobMasterDataAsync();
            //    var serializedData = JsonSerializer.Serialize(data);
            //    await _cache.SetStringAsync(CacheKey, serializedData, 
            //        new DistributedCacheEntryOptions
            //        {
            //            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            //        });
            //    _logger.LogInformation("Loaded from database.");
            //    return data;
            //}
            //_logger.LogInformation("Loaded from cache.");
            //return JsonSerializer.Deserialize<JobMasterDataResult?>(cacheData);
        }
    }
}
