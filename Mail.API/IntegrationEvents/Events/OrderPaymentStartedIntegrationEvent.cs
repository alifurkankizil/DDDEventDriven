using EventBus.Events;

namespace Mail.API.IntegrationEvents.Events;

public class OrderPaymentStartedIntegrationEvent : IntegrationEvent
{
    public OrderPaymentStartedIntegrationEvent(int orderId)
    {
        OrderId = orderId;
    }

    public int OrderId { get; set; }
}