using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _config;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration config)
        {
            _config = config;
            var factory = new ConnectionFactory() { HostName = _config["RabbitMQHost"], Port = int.Parse(_config["RabbitMQPort"]) };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += _connection_ConnectionShutdown;
                System.Console.WriteLine($"Connected to  Message Bus");
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"MessageBus Client connection came up with a issue : {ex}");
            }
        }

        private void _connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            System.Console.WriteLine($"Could not connect to the Message Bus");
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublish)
        {
            var message = JsonSerializer.Serialize(platformPublish);
            if (_connection.IsOpen)
            {
                System.Console.WriteLine($"--> RabbitMQ connection Open , sending message");
                SendMessage(message);
            }
            else
            {
                System.Console.WriteLine($"--> RabbitMQ connection ShutDown");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger",
                routingKey: "",
                basicProperties: null,
                body: body);
            System.Console.WriteLine($"We have sent a message : {message}");
        }

        public void Dispose()
        {
            System.Console.WriteLine("Disposing off now");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
