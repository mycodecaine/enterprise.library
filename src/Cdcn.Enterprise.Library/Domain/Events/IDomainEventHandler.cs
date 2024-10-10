using Cdcn.Enterprise.Library.Logging.Correlation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Events
{
    /// <summary>
    /// Represents a domain event handler interface.
    /// </summary>
    /// <typeparam name="TDomainEvent">The domain event type.</typeparam>
    public interface IDomainEventHandler<in TDomainEvent> : INotificationHandler<TDomainEvent>
        where TDomainEvent : IDomainEvent
    {
    }

    public abstract class BaseDomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent> where TDomainEvent : IDomainEvent
    {
        private readonly ICorrelationIdGenerator _correlationIdGenerator;

        protected BaseDomainEventHandler(ICorrelationIdGenerator correlationIdGenerator)
        {
            _correlationIdGenerator = correlationIdGenerator;
            CorrelationId = _correlationIdGenerator.Get();
        }

        public abstract Task Handle(TDomainEvent notification, CancellationToken cancellationToken);

        public string CorrelationId { get; }


    }



}
