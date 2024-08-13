using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.SeedWork;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrderingContext _context;

    public IUnitOfWork UnitOfWork
    {
        get
        {
            return _context;
        }
    }
    
    public OrderRepository(OrderingContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public Order Add(Order order)
    {
        return  _context.Orders.Add(order).Entity;
               
    }
}