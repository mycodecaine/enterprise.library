using Microsoft.Extensions.Logging;

namespace Cdcn.Enterprise.Library.Application.Mediator.Base
{
    /// <summary>
    /// QueryHandler is an abstract class that provides a template for handling query operations.
    /// It extends the BaseHandler class to leverage common handling and logging mechanisms.
    /// </summary>
    /// <typeparam name="TQuery">The type of the query being handled.</typeparam>
    /// <typeparam name="TResponse">The type of the response returned by the query.</typeparam>
    public abstract class QueryHandler<TQuery, TResponse> : BaseHandler<TResponse>, IQueryHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryHandler{TQuery, TResponse}"/> class.
        /// </summary>
        /// <param name="logger">The logger instance for logging operations.</param>
        protected QueryHandler(ILogger logger) : base(logger)
        {
        }

        /// <summary>
        /// Handles the query asynchronously.
        /// </summary>
        /// <param name="request">The query request to handle.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A Task representing the asynchronous operation that resolves to the response of type <typeparamref name="TResponse"/>.</returns>
        public abstract Task<TResponse> Handle(TQuery request, CancellationToken cancellationToken);
    }
}
