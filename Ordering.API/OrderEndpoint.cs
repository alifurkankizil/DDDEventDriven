using EventBus.Abstractions;
using EventBus.Events;
using MinimalApi.Endpoint;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.API;

public class OrderEndpoint : IEndpoint<IResult>
{
    private IOrderRepository _orderRepository;
    private readonly IEventBus _eventBus;

    public OrderEndpoint(IOrderRepository orderRepository, IEventBus eventBus)
    {
        _orderRepository = orderRepository;
        _eventBus = eventBus;
    }

    public async Task<IResult> HandleAsync()
    {
        var order = new Order("5440");
        _orderRepository.Add(order);
        await _orderRepository.UnitOfWork.SaveEntitiesAsync();
        IntegrationEvent paymentEvents =  new OrderPaymentStartedIntegrationEvent(order.Id);
        _eventBus.Publish(paymentEvents);
        return Results.Ok(order);
    }

    public void AddRoute(IEndpointRouteBuilder app)
    {
        app.MapPost("api/create-order", async () =>
        {
            var result = await HandleAsync();
            return result;
        });
    }
}