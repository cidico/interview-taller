using Interview.BackendApi.EventHub;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var port = configuration.GetValue<int>("PORT", 8080);

builder.Logging.AddConsole();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>((sp) =>
{
    var eventBus = new InMemoryEventBus();
    eventBus.Subscribe((SampleEvent @event) =>
    {
        Console.WriteLine($"Event Received: {JsonSerializer.Serialize(@event)}");
    });

    return eventBus;
});

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors("AllowAngular");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run($"http://0.0.0.0:{port}");