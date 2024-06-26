using api.Models;
using api.Services;
using api.Services.GenerateCostProfiles;

namespace api.Services.Observers;

public class DomainEvent
{
    public string EventType { get; set; } = null!;
    public object Data { get; set; } = null!;
}

public interface IDomainEventDispatcher
{
    void Dispatch(DomainEvent domainEvent);
}

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public DomainEventDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Dispatch(DomainEvent domainEvent)
    {
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
        var handlers = _serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            var handleMethod = handlerType.GetMethod("Handle");
            handleMethod?.Invoke(handler, new object[] { domainEvent });
        }
    }
}
