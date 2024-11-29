using Cdcn.Enterprise.Library.Application.Exceptions;
using Cdcn.Enterprise.Library.Domain.Exceptions;
using Microsoft.Extensions.Logging;


namespace Cdcn.Enterprise.Library.Application.Mediator.Base
{
    public abstract class BaseHandler<TResult>
    {
        private readonly ILogger _logger;

        public BaseHandler(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Handles an asynchronous operation safely by encapsulating the execution in a try-catch block 
        /// and logging any exceptions that occur during the operation.
        /// </summary>
        /// <param name="handleCoreAsync">
        /// A delegate representing the core asynchronous operation to execute.
        /// </param>
        /// <param name="context">
        /// An optional context string to include in the log messages, providing additional information about the operation.
        /// </param>
        /// <returns>
        /// A Task representing the asynchronous operation, which resolves to the result of type <typeparamref name="TResult"/>.
        /// </returns>
        /// <exception cref="ServiceInfrastructureException">
        /// Thrown when a <see cref="ServiceInfrastructureException"/> occurs during the operation.
        /// </exception>
        /// <exception cref="EnterpriseLibraryException">
        /// Thrown when an <see cref="EnterpriseLibraryException"/> occurs during the operation.
        /// </exception>
        /// <exception cref="ServiceApplicationException">
        /// Thrown for any other exceptions that occur, after logging and transforming the exception.
        /// </exception>
        protected async Task<TResult> HandleSafelyAsync(
            Func<Task<TResult>> handleCoreAsync, string context = "")
        {
            try
            {
                return await handleCoreAsync();
            }
            catch (ServiceInfrastructureException ex)
            {
                _logger?.LogError(ex, "An ServiceInfrastructureException occurred.");
                throw; // Re throw the original exception.
            }
            catch (EnterpriseLibraryException ex)
            {
                _logger?.LogError(ex, "An EnterpriseLibraryException occurred.");
                throw; // Re throw the original exception.
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "An ServiceApplicationException occurred.");
                throw HandleException.ThrowServiceApplicationException(ex, _logger, $"{context ?? this.GetType().FullName}");
            }
        }

        /// <summary>
        /// Safely handles an asynchronous operation by executing the core logic and managing exceptions.
        /// This method ensures that if the operation is canceled, a rollback (if provided) is executed, and proper logging is performed.
        /// </summary>
        /// <param name="handleCoreAsync">
        /// A delegate representing the core asynchronous operation to execute.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests during the operation.
        /// </param>
        /// <param name="rollBackAsync">
        /// An optional delegate representing a rollback operation to be executed if the operation is canceled.
        /// </param>
        /// <param name="context">
        /// An optional context string for logging purposes to identify the operation's source or purpose.
        /// </param>
        /// <returns>
        /// A Task representing the asynchronous operation that resolves to the result of type <typeparamref name="TResult"/>.
        /// </returns>
        /// <exception cref="OperationCanceledException">
        /// Thrown when the operation is canceled (either by the client or due to a timeout).
        /// </exception>
        /// <exception cref="ServiceInfrastructureException">
        /// Thrown when a <see cref="ServiceInfrastructureException"/> occurs.
        /// </exception>
        /// <exception cref="EnterpriseLibraryException">
        /// Thrown when an <see cref="EnterpriseLibraryException"/> occurs.
        /// </exception>
        /// <exception cref="ServiceApplicationException">
        /// Thrown when any other exceptions occur during the operation, after handling and logging them.
        /// </exception>
        protected async Task<TResult> HandleSafelyAsync(
            Func<Task<TResult>> handleCoreAsync, CancellationToken cancellationToken, Func<Task<TResult>>? rollBackAsync = null , string context = "")
        {
            try
            {
                return await handleCoreAsync();
            }
            catch (OperationCanceledException ex) when (cancellationToken.IsCancellationRequested)
            {
                _logger?.LogWarning("Operation was canceled.");

                // Execute rollback if it's provided
                if (rollBackAsync != null)
                {
                    _logger?.LogInformation("Executing rollback due to cancellation.");
                    await rollBackAsync();
                }

                throw HandleException.ThrowServiceApplicationException(ex, _logger, $"{context ?? this.GetType().FullName}");
            }
            catch (ServiceInfrastructureException ex)
            {
                _logger?.LogError(ex, "An ServiceInfrastructureException occurred.");
                throw; // Re throw the original exception.
            }
            catch (EnterpriseLibraryException ex)
            {
                _logger?.LogError(ex, "An EnterpriseLibraryException occurred.");
                throw; // Re throw the original exception.
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "An ServiceApplicationException occurred.");
                throw HandleException.ThrowServiceApplicationException(ex, _logger, $"{context ?? this.GetType().FullName}");
            }
        }
    }

}
