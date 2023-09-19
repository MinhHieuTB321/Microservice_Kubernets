
using System.Text;
using CommandWebService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandWebService.AsyncDataServices
{
    public class MessageBusSubcriber : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly IEventProcessor _eventProcessor;
        private IConnection? _connection;
        private IModel? _chanel;
        private string? _queueName;

        public MessageBusSubcriber(
            IConfiguration configuration,
            IEventProcessor eventProcessor)
        {
            _config=configuration;
            _eventProcessor=eventProcessor;
            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ(){
            var factory= new ConnectionFactory(){
                HostName=_config["RabbitMQHost"],
                Port=int.Parse(_config["RabbitMQPort"]!)
            };

            _connection=factory.CreateConnection();
            _chanel=_connection.CreateModel();
            _chanel.ExchangeDeclare(exchange:"trigger",type:ExchangeType.Fanout);
            _queueName=_chanel.QueueDeclare().QueueName;
            _chanel.QueueBind(queue:_queueName,
                exchange:"trigger",
                routingKey:"");

            Console.WriteLine("--> Listening on the Message Bus . . .");

            _connection.ConnectionShutdown+= RabbitMQ_ConnectionShutdown;
        }

        private void RabbitMQ_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection Shutdown");
        }

        public override void Dispose(){
            if(_chanel!.IsOpen)
            {
                _chanel.Close();
                _connection!.Close();
            }

            base.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer= new EventingBasicConsumer(_chanel);
            consumer.Received+= (ModuleHandle,ea)=>{
                Console.WriteLine("-->Event Received!");
                var body= ea.Body;
                var notificationMessage= Encoding.UTF8.GetString(body.ToArray());
                _eventProcessor.ProcessEvent(notificationMessage);
            };

            _chanel.BasicConsume(_queueName,autoAck:true,consumer:consumer);
            return Task.CompletedTask;
        }
    }
}