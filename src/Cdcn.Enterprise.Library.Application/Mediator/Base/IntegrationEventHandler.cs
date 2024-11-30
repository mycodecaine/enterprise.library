using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Mediator.Base
{
    public abstract class IntegrationEventHandler<TIntegrationEvent> : BaseHandler<TIntegrationEvent>, IIntegrationEventHandler<TIntegrationEvent> where TIntegrationEvent : IIntegrationEvent
    {
        public IntegrationEventHandler(ILogger logger) : base(logger)
        {
        }

        public abstract Task Handle(TIntegrationEvent notification, CancellationToken cancellationToken);
        
    }

}
