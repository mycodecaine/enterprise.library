using MediatR;

namespace Cdcn.Enterprise.Library.Domain.Events
{
    /// <summary>
    /// Represents a domain event in the system. 
    /// Implementing this interface allows the event to be published 
    /// and handled using the Mediator pattern provided by MediatR.
    /// </summary>
    public interface IDomainEvent : INotification
    {
    }    
}
