﻿using Cdcn.Enterprise.Library.Application.Mediator.Events;

namespace Cdcn.Enterprise.Library.Application.Mediator
{
    /// <summary>
    /// Represents the integration event publisher interface.
    /// </summary>
    public interface IIntegrationEventPublisher
    {
        /// <summary>
        /// Publishes the specified integration event to the message queue.
        /// </summary>
        /// <param name="integrationEvent">The integration event.</param>
        /// <returns>The completed task.</returns>
        void Publish(IIntegrationEvent integrationEvent);
    }
}
