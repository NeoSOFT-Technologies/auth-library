using AuthLibrary.IServices;
using AuthLibrary.Models.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;

namespace AuthLibrary.Services
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _cacheOptions;
        public MemoryCacheService(IMemoryCache memoryCache, IOptions<CacheConfiguration> cacheConfig)
        {
            _memoryCache = memoryCache;
            var _cacheConfig = cacheConfig.Value;
            if (_cacheConfig != null)
            {
                _cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = _cacheConfig.AbsoluteExpirationInMinutes > 0 ? DateTime.Now.AddMinutes(_cacheConfig.AbsoluteExpirationInMinutes) : DateTime.Now.AddMinutes(30),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = _cacheConfig.SlidingExpirationInMinutes > 0 ? TimeSpan.FromMinutes(_cacheConfig.SlidingExpirationInMinutes) : TimeSpan.FromMinutes(5)
                };
            }
        }
        public bool TryGet<T>(string cacheKey, out T value)
        {
            _memoryCache.TryGetValue(cacheKey, out value);
            if (value == null) return false;
            else return true;
        }
        public T Set<T>(string cacheKey, T value)
        {
            return _memoryCache.Set(cacheKey, value, _cacheOptions);
        }
        public void Remove(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }
    }
}
