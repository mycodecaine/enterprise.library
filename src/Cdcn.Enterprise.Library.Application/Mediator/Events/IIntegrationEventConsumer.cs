namespace Cdcn.Enterprise.Library.Application.Mediator.Events
{
    /// <summary>
    /// Defines a contract for consuming integration events.
    /// </summary>
    public interface IIntegrationEventConsumer
    {
        /// <summary>
        /// Consumes the incoming the specified integration event.
        /// </summary>
        void Consume(IIntegrationEvent integrationEvent);
    }
}
