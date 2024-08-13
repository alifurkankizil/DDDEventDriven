using Ordering.Domain.SeedWork;

namespace Ordering.Domain.AggregatesModel.OrderAggregate;

public class OrderStatus : Enumeration
{
    public static OrderStatus InProcess = new OrderStatus(1, nameof(InProcess).ToLowerInvariant());
    public static OrderStatus Shipped = new OrderStatus(2, nameof(Shipped).ToLowerInvariant());
    public static OrderStatus Canceled = new OrderStatus(3, nameof(Canceled).ToLowerInvariant());
    
    protected OrderStatus()
    {
    }

    public OrderStatus(int id, string name)
        : base(id, name)
    {
    }
    
    public static IEnumerable<OrderStatus> List()
    {
        return new[] { InProcess, Shipped, Canceled };
    }
}