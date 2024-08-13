using System.Text;
using System.Text.Json;
using EventBus.Abstractions;
using EventBus.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EventBusRabbitMQ;

public class EventBusRabbitMQ : IEventBus, IDisposable
{
    private readonly string _brokerName = "eshop_event_bus";
    //private readonly string _connectionString;
    private readonly Dictionary<string, List<IIntegrationEventHandler>>  _handlers;
    private readonly List<Type> _eventTypes;
    
    private IModel _model;
    private IConnection _connection;
    private string _queueName;

    public EventBusRabbitMQ(string connectionString)
    {
        //_connectionString = connectionString;
        _handlers = new Dictionary<string, List<IIntegrationEventHandler>>();
        _eventTypes = new List<Type>();
    }
    
    public void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent
    {
        var eventName = typeof(T).Name;
        if (_handlers.ContainsKey(eventName))   
        {
            _handlers[eventName].Add(handler);
        }
        else
        {
            var channel = GetChannel();
            channel.QueueBind(queue: _queueName,
                exchange: _brokerName,
                routingKey: eventName);
                                               
            _handlers.Add(eventName, new List<IIntegrationEventHandler>());
            _handlers[eventName].Add(handler);
            _eventTypes.Add(typeof(T));
        }
    }

    public void Unsubscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent
    {
        throw new NotImplementedException();
    }

    public void Publish(IntegrationEvent @event)
    {
        var eventName = @event.GetType().Name;
        //TODO : Manuel
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Password = "guest",
            UserName = "guest"
        };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange: _brokerName,
                type: "direct");

            string message = JsonSerializer.Serialize(@event);                                
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: _brokerName,
                routingKey: eventName,
                basicProperties: null,
                body: body);                
        }
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
    
    private IModel GetChannel()
    {
        if (_model != null)
        {
            return _model;
        }
        else
        {
            (_model, _connection) = CreateConnection();
            return _model;
        }
    }
    
    private (IModel model, IConnection connection) CreateConnection()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Password = "guest",
            UserName = "guest"
        };
        var con = factory.CreateConnection();
        var channel = con.CreateModel();

        channel.ExchangeDeclare(exchange: _brokerName,
            type: "direct");
        if (string.IsNullOrEmpty(_queueName))
        {
            _queueName = channel.QueueDeclare().QueueName;
        }

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var eventName = ea.RoutingKey;
            var message = Encoding.UTF8.GetString(ea.Body.ToArray());

            await ProcessEvent(eventName, message);
        };
        channel.BasicConsume(queue: _queueName,
            autoAck: true, 
            consumer: consumer);

        return (channel, con);
    }
    
    private async Task ProcessEvent(string eventName, string message)
    {
        if (_handlers.ContainsKey(eventName))
        {
            Type eventType = _eventTypes.Single(t => t.Name == eventName);
            var integrationEvent = JsonSerializer.Deserialize(message, eventType);                
            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
            var handlers = _handlers[eventName];

            foreach (var handler in handlers)
            {
                await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
            }
        }
    }
    
}