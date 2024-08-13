using EventBus.Events;

namespace Ordering.API;

public class OrderPaymentStartedIntegrationEvent : IntegrationEvent
{
    public OrderPaymentStartedIntegrationEvent(int orderId)
    {
        OrderId = orderId;
    }

    public int OrderId { get; set; }
}