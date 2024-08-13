using MediatR;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.AggregatesModel.OrderAggregate;
using Ordering.Domain.SeedWork;

namespace Ordering.Infrastructure;

public class OrderingContext : DbContext,IUnitOfWork
{
    public DbSet<Order> Orders { get; set; }
    
    private readonly IMediator _mediator;
    
    public OrderingContext(DbContextOptions options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("MyDatabase");
    }
    
    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        await _mediator.DispatchDomainEventsAsync(this);


        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed thought the DbContext will be commited
        var result = await base.SaveChangesAsync();

        return true;
    }
}