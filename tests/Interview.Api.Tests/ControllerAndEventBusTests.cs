using Interview.BackendApi.Controllers;
using Interview.BackendApi.EventHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Text.Json;

namespace Interview.Api.Tests;
public class ControllerAndEventBusTests
{
    private readonly ILogger<SampleController> _logger;
    private readonly IMemoryCache _cache;
    private readonly InMemoryEventBus _eventBus;
    private readonly SampleController _controller;

    public ControllerAndEventBusTests()
    {
        _logger = Substitute.For<ILogger<SampleController>>();
        _cache = new MemoryCache(new MemoryCacheOptions());
        _eventBus = new InMemoryEventBus();

        _controller = new SampleController(_logger, _cache, _eventBus);
    }

    [Fact]
    public void Get_ReturnsValue_WhenKeyExists()
    {
        var value = JsonSerializer.Serialize("testValue");
        _cache.Set("TESTKEY", "testValue");

        var result = _controller.Get("testKey") as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Get_ReturnsNotFound_WhenKeyDoesNotExist()
    {
        var result = _controller.Get("nonExistentKey") as NotFoundResult;

        Assert.NotNull(result);
    }

    [Fact]
    public void Post_StoresValueAndPublishesEvent()
    {
        var keyValue = new KeyValuePair<string, string>("NEWKEY", "newValue");
        var actionMock = Substitute.For<Action<SampleEvent>>();
        
        _eventBus.Subscribe(actionMock);

        var result = _controller.Post(keyValue) as OkResult;

        Assert.NotNull(result);
        Assert.True(_cache.TryGetValue("NEWKEY", out string? value));
        Assert.Equal("newValue", value);
        actionMock.Received(1).Invoke(Arg.Any<SampleEvent>());
    }

    [Fact]
    public void EventBus_PublishesAndSubscribesCorrectly()
    {
        var handlerCalled = false;
        _eventBus.Subscribe<SampleEvent>(e => handlerCalled = true);

        var sampleEvent = new SampleEvent { Message = "Test Event" };
        _eventBus.Publish(sampleEvent);

        Assert.True(handlerCalled);
    }
}