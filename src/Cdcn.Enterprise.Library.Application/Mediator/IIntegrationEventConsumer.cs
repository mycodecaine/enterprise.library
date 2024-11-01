using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Mediator
{
    public interface IIntegrationEventConsumer
    {
        /// <summary>
        /// Consumes the incoming the specified integration event.
        /// </summary>
        void Consume(IIntegrationEvent integrationEvent);
    }
}
