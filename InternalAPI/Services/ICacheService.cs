using Microsoft.Extensions.Caching.Memory;

namespace InternalAPI.Services;

public interface ICacheService<T>
{
    Task<T> Get(string key);
    void Set(string key, T entry, MemoryCacheEntryOptions options = null);
    void Remove(string key);
}