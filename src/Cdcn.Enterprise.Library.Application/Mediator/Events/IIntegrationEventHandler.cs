using MediatR;

namespace Cdcn.Enterprise.Library.Application.Mediator.Events
{
    /// <summary>
    /// Interface for handling integration events.
    /// </summary>
    /// <typeparam name="TIntegrationEvent">The type of the integration event.</typeparam>
    public interface IIntegrationEventHandler<in TIntegrationEvent> : INotificationHandler<TIntegrationEvent>
        where TIntegrationEvent : IIntegrationEvent
    {
    }
}
