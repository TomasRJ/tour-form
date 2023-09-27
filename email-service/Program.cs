using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost", VirtualHost = "ucl" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "Tour", type: ExchangeType.Topic);

var dictionary = new Dictionary<string, object>
{
    { "x-dead-letter-exchange", "Tour" }
};

// declare a server-named queue
var queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(queue: queueName, exchange: "Tour", routingKey: "Tour.Booked");
var counter = 0;

var textInput = " ";
while(textInput != "")
{ 
    Console.WriteLine(" [*] Waiting for messages. To exit just press enter");
    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {    
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Random rnd = new Random(1);
        int num = rnd.Next(1, 101);
        if (num < 90) throw new DivideByZeroException();
        Console.WriteLine($"{counter++} {message} {DateTime.Now}");
        channel.BasicAck(ea.DeliveryTag, false);   
        channel.BasicNack(ea.DeliveryTag, false, true);

    };

    var response = channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
    Console.WriteLine(JsonSerializer.Serialize(response));

    textInput = Console.ReadLine();
}