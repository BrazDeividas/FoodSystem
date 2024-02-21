using Microsoft.Extensions.Caching.Memory;

namespace InternalAPI.Services;

public class MemoryCacheService<T> : ICacheService<T>
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public virtual Task<T> Get(string key)
    {
        T entry;
        
        _memoryCache.TryGetValue(key, out entry);

        return Task.FromResult(entry);
    }

    public void Set(string key, T entry, MemoryCacheEntryOptions options = null)
    {
        _memoryCache.Set(key, entry, options);
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}