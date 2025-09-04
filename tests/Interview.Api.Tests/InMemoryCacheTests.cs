using Microsoft.Extensions.Caching.Memory;

namespace Interview.Api.Tests;
public class InMemoryCacheTests
{
    private readonly IMemoryCache _cache;

    public InMemoryCacheTests()
    {
        _cache = new MemoryCache(new MemoryCacheOptions());
    }

    [Fact]
    public void TestInMemoryCache()
    {
        var cache = new MemoryCache(new MemoryCacheOptions());
        cache.Set("key", "value");

        Assert.True(cache.TryGetValue("key", out string? value));
        Assert.Equal("value", value);
    }
}
