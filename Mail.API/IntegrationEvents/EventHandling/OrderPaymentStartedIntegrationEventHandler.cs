using EventBus.Abstractions;
using Mail.API.IntegrationEvents.Events;

namespace Mail.API.IntegrationEvents.EventHandling;

public class OrderPaymentStartedIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentStartedIntegrationEvent>
{
    public Task Handle(OrderPaymentStartedIntegrationEvent @event)
    {
        return Task.CompletedTask;
    }
}