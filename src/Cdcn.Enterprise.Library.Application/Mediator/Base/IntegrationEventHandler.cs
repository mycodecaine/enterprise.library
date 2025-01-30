using Microsoft.Extensions.Logging;

namespace Cdcn.Enterprise.Library.Application.Mediator.Base
{
    /// <summary>
    /// IntegrationEventHandler is an abstract class that provides a template for handling integration events.
    /// It extends the BaseHandler class to leverage common handling and logging mechanisms.
    /// </summary>
    /// <typeparam name="TIntegrationEvent">The type of the integration event.</typeparam>
    public abstract class IntegrationEventHandler<TIntegrationEvent> : BaseHandler<TIntegrationEvent>, IIntegrationEventHandler<TIntegrationEvent> where TIntegrationEvent : IIntegrationEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventHandler{TIntegrationEvent}"/> class.
        /// </summary>
        /// <param name="logger">The logger instance to be used for logging.</param>
        public IntegrationEventHandler(ILogger logger) : base(logger)
        {
        }

        /// <summary>
        /// Handles the integration event asynchronously.
        /// This method must be implemented by derived classes to define the specific handling logic.
        /// </summary>
        /// <param name="notification">The integration event to handle.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public abstract Task Handle(TIntegrationEvent notification, CancellationToken cancellationToken);
    }

}
