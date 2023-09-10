using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "Tour", type: ExchangeType.Topic);
// declare a server-named queue
var queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(queue: queueName, exchange: "Tour", routingKey: "Tour.*");

var textInput = " ";
while(textInput != "")
{ 
    Console.WriteLine(" [*] Waiting for messages. To exit just press enter");
    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($"{message}");
    };
    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    textInput = Console.ReadLine();
}