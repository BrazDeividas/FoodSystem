using Microsoft.Extensions.Caching.Memory;

namespace FoodSystemAPI.Services;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public virtual Task<T> Get<T>(string key)
    {
        _memoryCache.TryGetValue(key, out T entry);

        return Task.FromResult(entry);
    }

    public void Set(string key, object entry, MemoryCacheEntryOptions options = null)
    {
        _memoryCache.Set(key, entry, options);
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}