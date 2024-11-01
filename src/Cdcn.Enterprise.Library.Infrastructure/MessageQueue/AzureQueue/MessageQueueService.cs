using Azure.Storage.Queues;
using Cdcn.Enterprise.Library.Application.Core.Abstraction.MessageQueue;
using Cdcn.Enterprise.Library.Application.Mediator;
using Cdcn.Enterprise.Library.Infrastructure.MessageQueue.AzureQueue.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Cdcn.Enterprise.Library.Infrastructure.MessageQueue.AzureQueue
{
    public class MessageQueueService : IMessageQueueService
    {
        private readonly AzureQueueSetting _azureQueueSetting;
        private readonly QueueClient _queueClient;
        private const string QueueName = "sportivo";

        public MessageQueueService(IOptions<AzureQueueSetting> azureQueueSetting)
        {
            _azureQueueSetting = azureQueueSetting.Value;
            _queueClient = new QueueClient(_azureQueueSetting.ConnectionString, QueueName);
        }

        private async Task Publish(byte[] request)
        {
            await _queueClient.CreateIfNotExistsAsync();
            if (_queueClient.Exists())
            {
                await _queueClient.SendMessageAsync(Convert.ToBase64String(request));
            }
        }

        private async Task Publish(byte[] request, string queue)
        {
            var queueClient = new QueueClient(_azureQueueSetting.ConnectionString, queue);
            await queueClient.CreateIfNotExistsAsync();
            if (queueClient.Exists())
            {
                await queueClient.SendMessageAsync(Convert.ToBase64String(request));
            }

        }
        public async Task Publish<Req>(Req request) where Req : IIntegrationEvent
        {
            
            string payload = JsonConvert.SerializeObject(request, typeof(IIntegrationEvent), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            byte[] body = Encoding.UTF8.GetBytes(payload);

            await Publish(body);

        }

        public async Task Publish<Req>(Req request, string queue) where Req : IIntegrationEvent
        {
            string payload = JsonConvert.SerializeObject(request, typeof(IIntegrationEvent), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            byte[] body = Encoding.UTF8.GetBytes(payload);

            await Publish(body,queue);
        }
    }
}
