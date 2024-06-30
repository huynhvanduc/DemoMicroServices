using Contract.Common.Interfaces;
using Contract.Messages;
using RabbitMQ.Client;
using System.Text;

namespace Infrastructure.Messages;

public class RabbitMQProducer : IMessageProducer
{
    private readonly ISerializeService _service;

    public RabbitMQProducer(ISerializeService service)
    {
        _service = service;
    }

    public void SendMessage<T>(T message)
    {
        var connectionFactory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        var connection = connectionFactory.CreateConnection();
        using var chanel = connection.CreateModel();

        chanel.QueueDeclare("orders", exclusive: false);

        var jsonData = _service.Serialize(message);  

        var body = Encoding.UTF8.GetBytes(jsonData);

        chanel.BasicPublish(exchange: "", routingKey: "orders", body: body);
    }
}
