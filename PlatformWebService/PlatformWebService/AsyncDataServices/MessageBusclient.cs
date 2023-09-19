using System.Text;
using System.Text.Json;
using PlatformWebService.DTOs;
using RabbitMQ.Client;

namespace PlatformWebService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _config;
        private readonly IConnection? _connection;
        private readonly IModel? _chanel;

        public MessageBusClient(IConfiguration configuration)
        {
            _config=configuration;
            var factory= new ConnectionFactory()
            {
                HostName=_config["RabbitMQHost"],
                Port=int.Parse(_config["RabbitMQPort"]!)
            };
            try
            {
                _connection= factory.CreateConnection();
                _chanel= _connection.CreateModel();
                _chanel.ExchangeDeclare(exchange:"trigger",type:ExchangeType.Fanout);
                _connection.ConnectionShutdown+= RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connection to MessageBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"-->Could not connect to the Message Bus:{ex.Message}");            }
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"--> Rabbit MQ Connection Shutdown");
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message= JsonSerializer.Serialize(platformPublishedDto);

            if(_connection!.IsOpen)
            {
                Console.WriteLine("RabbitMQ Connection Open, Sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("RabbitMQ Connection Closed, Not sending message...");
            }
        }

        private void SendMessage(string message)
        {
            var body= Encoding.UTF8.GetBytes(message);
            _chanel.BasicPublish(exchange:"trigger",
                            routingKey:"",
                            basicProperties:null,
                            body);
            Console.WriteLine($"--> We have sennt {message}");
        }

        public void Dispose(){
            Console.WriteLine("MessageBus Disposed");
            if(_chanel!.IsOpen){
                _chanel.Close();
                _connection!.Close();
            }
        }
    }
}