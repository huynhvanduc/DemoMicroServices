using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var connectionFactory = new ConnectionFactory
{
    HostName = "localhost"
};

var connection = connectionFactory.CreateConnection();
using var chanel = connection.CreateModel();
chanel.QueueDeclare("orders", exclusive: false);

var consumer = new EventingBasicConsumer(chanel);
consumer.Received += (_, eventArgs) =>
{
    var body = eventArgs.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Message received: {message}");
};

chanel.BasicConsume(queue: "orders", autoAck: true, consumer: consumer);
Console.ReadKey();