using Ordering.Domain.AggregatesModel.Events;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.AggregatesModel.OrderAggregate;

public class Order : Entity,IAggregateRoot
{
    private DateTime _orderDate;
    public OrderStatus OrderStatus { get; private set; }
    private int _orderStatusId;
    
    protected Order() { }
    
    public Order(string cardNumber)
    {
        _orderDate = DateTime.UtcNow;
        _orderStatusId = OrderStatus.InProcess.Id;
        // Add the OrderStarterDomainEvent to the domain events collection 
        // to be raised/dispatched when comitting changes into the Database [ After DbContext.SaveChanges() ]
        AddOrderStartedDomainEvent(cardNumber);
    }
    
    private void AddOrderStartedDomainEvent(string cardNumber)
    {
        var orderStartedDomainEvent = new OrderStartedDomainEvent(
            this, cardNumber);

        this.AddDomainEvent(orderStartedDomainEvent);
    }
}