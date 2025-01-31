using Cdcn.Enterprise.Library.Application.Mediator;

namespace Cdcn.Enterprise.Library.Application.Core.Abstraction.MessageQueue
{
    /// <summary>
    /// Interface for message queue service to publish integration events.
    /// </summary>
    public interface IMessageQueueService
    {
        const string DefaultQueueName = "Cdcn";

        /// <summary>
        /// Publishes an integration event to the default queue.
        /// </summary>
        /// <typeparam name="Req">The type of the integration event.</typeparam>
        /// <param name="request">The integration event to publish.</param>
        /// <returns>A task that represents the asynchronous publish operation.</returns>
        Task Publish<Req>(Req request) where Req : IIntegrationEvent;

        /// <summary>
        /// Publishes an integration event to a specified queue.
        /// </summary>
        /// <typeparam name="Req">The type of the integration event.</typeparam>
        /// <param name="request">The integration event to publish.</param>
        /// <param name="queue">The name of the queue to publish the event to.</param>
        /// <returns>A task that represents the asynchronous publish operation.</returns>
        Task Publish<Req>(Req request, string queue = DefaultQueueName) where Req : IIntegrationEvent;
    }
}
