using Cdcn.Enterprise.Library.Domain.Common;
using Cdcn.Enterprise.Library.Domain.Events;
using MediatR;

namespace Cdcn.Enterprise.Library.Application.Mediator
{
    /// <summary>
    /// Represents the marker interface for an integration event.
    /// </summary>
    public interface IIntegrationEvent : INotification, ICorrelation
    {
    }

    public abstract class BaseIntegrationEvent : IIntegrationEvent
    {
        public abstract Guid CorrelationId { get; }
    }
}
