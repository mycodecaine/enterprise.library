using Cdcn.Enterprise.Library.Application.Exceptions;
using Cdcn.Enterprise.Library.Domain.Exceptions;
using Microsoft.Extensions.Logging;


namespace Cdcn.Enterprise.Library.Application.Mediator.Base
{
    public abstract class BaseHandler<TRequest, TResult>
    {
        protected async Task<TResult> HandleSafelyAsync(
            Func<Task<TResult>> handleCoreAsync,
            ILogger logger,string context="")
        {
            try
            {
                return await handleCoreAsync();
            }
            catch (ServiceInfrastructureException ex)
            {
                logger?.LogError(ex, "An ServiceInfrastructureException occurred.");
                throw; // Re throw the original exception.
            }
            catch (EnterpriseLibraryException ex)
            {
                logger?.LogError(ex, "An EnterpriseLibraryException occurred.");
                throw; // Re throw the original exception.
            }
            catch (Exception ex)
            {                
                logger?.LogError(ex, "An unexpected error occurred.");
                throw HandleException.ThrowServiceApplicationException(ex, logger, $"{context ?? this.GetType().FullName}");
            }
        }
    }

}
