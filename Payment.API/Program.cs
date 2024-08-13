using EventBus.Abstractions;
using Payment.API.IntegrationEvents.EventHandling;
using Payment.API.IntegrationEvents.Events;
using EventBusRabbitMQ;
using MinimalApi.Endpoint.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

builder.Services.AddEndpoints();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IIntegrationEventHandler<OrderPaymentStartedIntegrationEvent>, OrderPaymentStartedIntegrationEventHandler>();
builder.Services.AddSingleton<IEventBus>(new EventBusRabbitMQ.EventBusRabbitMQ("")); 


var app = builder.Build();

var catalogPriceHandler = app.Services.GetService<IIntegrationEventHandler<OrderPaymentStartedIntegrationEvent>>();
var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe(catalogPriceHandler);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// var catalogPriceHandler = app.Services.GetService<IIntegrationEventHandler<OrderPaymentStartedIntegrationEvent>>();
// var eventBus = app.Services.GetRequiredService<IEventBus>();
// eventBus.Subscribe<OrderPaymentStartedIntegrationEvent>(catalogPriceHandler);


app.UseHttpsRedirection();


app.Run();
