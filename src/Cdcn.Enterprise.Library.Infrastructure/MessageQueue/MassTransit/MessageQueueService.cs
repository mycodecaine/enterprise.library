using Cdcn.Enterprise.Library.Application.Core.Abstraction.MessageQueue;
using Cdcn.Enterprise.Library.Application.Mediator.Events;
using Cdcn.Enterprise.Library.Infrastructure.MessageQueue.MassTransit.Settings;
using MassTransit;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Cdcn.Enterprise.Library.Infrastructure.MessageQueue.MassTransit
{
    public class MessageQueueService : IMessageQueueService
    {
        private readonly MassTransitSetting _setting;
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public MessageQueueService(IOptions<MassTransitSetting> setting, ISendEndpointProvider sendEndpointProvider)
        {
            _setting = setting.Value;
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task Publish<Req>(Req request) where Req : IIntegrationEvent
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("rabbitmq://127.0.0.1/sportivo"));

            string payload = JsonConvert.SerializeObject(request, typeof(IIntegrationEvent), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            byte[] body = Encoding.UTF8.GetBytes(payload);

            await endpoint.Send(body);

            // return Task.CompletedTask;
        }

        public async Task Publish<Req>(Req request, string queue) where Req : IIntegrationEvent
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queue}"));

            string payload = JsonConvert.SerializeObject(request, typeof(IIntegrationEvent), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            byte[] body = Encoding.UTF8.GetBytes(payload);

            await endpoint.Send(body);
        }
    }
}
