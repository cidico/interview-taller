using Interview.BackendApi.EventHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace Interview.BackendApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SampleController : ControllerBase
{
    private readonly ILogger<SampleController> _logger;
    private readonly IMemoryCache _cache;
    private readonly IEventBus _eventBus;

    public SampleController(ILogger<SampleController> logger, IMemoryCache cache, IEventBus eventBus)
    {
        _logger = logger;
        _cache = cache;
        _eventBus = eventBus;
    }

    [HttpGet("{key}")]
    public IActionResult Get(string key)
    {
        if (_cache.TryGetValue(key.ToUpper(), out string? value))
        {
            _logger.LogInformation($"Value from {key.ToUpper()}: {value}");
            _eventBus.Publish(new SampleEvent { Message = $"Get: {key.ToUpper()} = {value}" });
            
            return Ok(JsonSerializer.Serialize(value));
        }

        return NotFound();
    }

    [HttpPost]
    public IActionResult Post([FromBody] KeyValuePair<string, string> item)
    {
        var key = item.Key.ToUpper();

        _cache.Set(key, item.Value, TimeSpan.FromMinutes(10));
        _logger.LogInformation($"Item added: {key} = {item.Value.ToUpper()}");
        _eventBus.Publish(new SampleEvent { Message = $"Post: {key} = {item.Value.ToUpper()}" });

        return Ok();
    }
}