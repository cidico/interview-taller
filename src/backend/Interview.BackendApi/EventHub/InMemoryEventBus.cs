namespace Interview.BackendApi.EventHub;

public class InMemoryEventBus : IEventBus
{
    private readonly Dictionary<Type, List<Delegate>> _handlers = new();

    public InMemoryEventBus()
    {
    }

    public void Publish<T>(T @event) where T : class
    {
        if (_handlers.TryGetValue(typeof(T), out var handlers))
        {
            foreach (var handler in handlers)
            {
                ((Action<T>)handler)(@event);
            }
        }
    }

    public void Subscribe<T>(Action<T> handler) where T : class
    {
        var type = typeof(T);

        if (!_handlers.ContainsKey(type))
        {
            _handlers[type] = new List<Delegate>();
        }

        _handlers[type].Add(handler);
    }
}
