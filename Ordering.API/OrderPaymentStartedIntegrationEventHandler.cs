using EventBus.Abstractions;

namespace Ordering.API;

public class OrderPaymentStartedIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentStartedIntegrationEvent>
{
    public Task Handle(OrderPaymentStartedIntegrationEvent @event)
    {
        throw new NotImplementedException();
    }
}