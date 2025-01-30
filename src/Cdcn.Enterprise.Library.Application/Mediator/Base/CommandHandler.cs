using Microsoft.Extensions.Logging;

namespace Cdcn.Enterprise.Library.Application.Mediator.Base
{
    /// <summary>
    /// CommandHandler is an abstract class that provides a template for handling commands in a CQRS pattern.
    /// It extends the BaseHandler class to leverage safe handling of asynchronous operations and implements the ICommandHandler interface.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command being handled.</typeparam>
    /// <typeparam name="TResponse">The type of the response returned by the command handler.</typeparam>
    public abstract class CommandHandler<TCommand, TResponse> : BaseHandler<TResponse>, ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        /// <summary>
        /// Initializes a new instance of the CommandHandler class with the specified logger.
        /// </summary>
        /// <param name="logger">The logger to be used for logging operations.</param>
        protected CommandHandler(ILogger logger) : base(logger)
        {

        }

        /// <summary>
        /// Handles the specified command asynchronously.
        /// This method must be implemented by derived classes to define the command handling logic.
        /// </summary>
        /// <param name="request">The command to handle.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A Task representing the asynchronous operation that resolves to the response of type <typeparamref name="TResponse"/>.</returns>
        public abstract Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken);
    }
}
