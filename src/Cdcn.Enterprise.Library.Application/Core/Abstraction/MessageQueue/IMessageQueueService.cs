using Cdcn.Enterprise.Library.Application.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Core.Abstraction.MessageQueue
{
    public interface IMessageQueueService
    {
        Task Publish<Req>(Req request) where Req : IIntegrationEvent;
        Task Publish<Req>(Req request, string queue) where Req : IIntegrationEvent;
    }
}
