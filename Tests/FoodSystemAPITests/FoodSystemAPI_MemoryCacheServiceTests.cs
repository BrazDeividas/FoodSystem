using FoodSystemAPI.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Tests.FoodSystemAPITests;

internal class FoodSystemAPI_MemoryCacheServiceTests
{
    private MemoryCacheService _memoryCache = null!;

    [OneTimeSetUp]
    public void Init()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();

        var memoryCache = serviceProvider.GetService<IMemoryCache>();
        _memoryCache = new MemoryCacheService(memoryCache);
    }

    [Test, Order(1)]
    public void Set_ValidKeyValidValue_ValueIsCached()
    {

        var key = "test";
        string[] value = ["value1", "value2"];

        _memoryCache.Set(key, value);
    }

    [Test, Order(2)]
    public async Task Get_ValidKey_ValueIsRetrieved()
    {
        var key = "test";
        string[] value = ["value1", "value2"];

        var result = await _memoryCache.Get<string[]>(key);

        Assert.That(result, Is.EqualTo(value));
    }

    [Test, Order(3)]
    public async Task Remove_ValidKey_ValueIsRemoved()
    {
        var key = "test";

        _memoryCache.Remove(key);

        var result = await _memoryCache.Get<string[]>(key);

        Assert.That(result, Is.Null);
    }
}
