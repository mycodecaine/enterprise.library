using Cdcn.Enterprise.Library.Application.Exceptions;
using Cdcn.Enterprise.Library.Domain.Exceptions;
using Microsoft.Extensions.Logging;


namespace Cdcn.Enterprise.Library.Application.Mediator.Base
{
    public class BaseHandler<TRequest, TResult>
    {
        private readonly ILogger _logger;

        public BaseHandler(ILogger logger)
        {
            _logger = logger;
        }

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
                _logger?.LogError(ex, "An unexpected error occurred.");
                throw HandleException.ThrowServiceApplicationException(ex, _logger, $"{context ?? this.GetType().FullName}");
            }
        }
    }

}
