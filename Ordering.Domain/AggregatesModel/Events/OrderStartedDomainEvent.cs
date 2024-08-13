using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Domain.AggregatesModel.Events;

public class OrderStartedDomainEvent : INotification
{
    public OrderStartedDomainEvent(Order order,string cardNumber)
    {
        Order = order;
        CardNumber = cardNumber;
    }

    public Order Order { get; private set; }
    public string CardNumber { get; private set; }
}