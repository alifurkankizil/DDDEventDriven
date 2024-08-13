using EventBus.Abstractions;
using Payment.API.IntegrationEvents.Events;

namespace Payment.API.IntegrationEvents.EventHandling;

public class OrderPaymentStartedIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentStartedIntegrationEvent>
{
    public Task Handle(OrderPaymentStartedIntegrationEvent @event)
    {
        return Task.CompletedTask;
    }
}