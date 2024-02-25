using Microsoft.Extensions.Caching.Memory;

namespace FoodSystemAPI.Services;

public interface ICacheService
{
    Task<T> Get<T>(string key);
    void Set(string key, object entry, MemoryCacheEntryOptions options = null);
    void Remove(string key);
}