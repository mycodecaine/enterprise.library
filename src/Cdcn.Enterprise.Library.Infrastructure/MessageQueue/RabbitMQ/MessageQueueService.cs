using Cdcn.Enterprise.Library.Application.Core.Abstraction.MessageQueue;
using Cdcn.Enterprise.Library.Application.Mediator.Events;
using Cdcn.Enterprise.Library.Infrastructure.MessageQueue.RabbitMQ.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Cdcn.Enterprise.Library.Infrastructure.MessageQueue.RabbitMQ
{
    public class MessageQueueService : IMessageQueueService
    {
        private readonly RabbitMqSetting _messageBrokerSettings;
        private readonly IConnection _connection;
        private readonly IModel _channel;


        public MessageQueueService(IOptions<RabbitMqSetting> messageBrokerSettingsOptions)
        {
            _messageBrokerSettings = messageBrokerSettingsOptions.Value;

            IConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = _messageBrokerSettings.HostName,
                Port = _messageBrokerSettings.Port,
                UserName = _messageBrokerSettings.UserName,
                Password = _messageBrokerSettings.Password,
                Uri = new Uri(_messageBrokerSettings.Url)
            };

            _connection = connectionFactory.CreateConnection();

            _channel = _connection.CreateModel();            

            _channel.QueueDeclare(_messageBrokerSettings.QueueName, true, false, false);

            
        }
        public  Task Publish<Req>(Req request) where Req : IIntegrationEvent
        {
            string payload = JsonConvert.SerializeObject(request, typeof(IIntegrationEvent), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            byte[] body = Encoding.UTF8.GetBytes(payload);

            _channel.BasicPublish(string.Empty, _messageBrokerSettings.QueueName, body: body);

            return Task.CompletedTask;
        }

        public  Task Publish<Req>(Req request, string queue) where Req : IIntegrationEvent
        {
            string payload = JsonConvert.SerializeObject(request, typeof(IIntegrationEvent), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            byte[] body = Encoding.UTF8.GetBytes(payload);

            _channel.BasicPublish(string.Empty, queue, body: body);

            return Task.CompletedTask;
        }
    }
}
