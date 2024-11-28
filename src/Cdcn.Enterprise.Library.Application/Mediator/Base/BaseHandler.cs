using Cdcn.Enterprise.Library.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Mediator.Base
{
    public abstract class BaseHandler<TRequest, TResult>
    {
        protected async Task<TResult> HandleWithExceptionHandlingAsync(
            Func<Task<TResult>> handleCoreAsync,
            ILogger logger)
        {
            try
            {
                return await handleCoreAsync();
            }
            catch (EnterpriseLibraryException ex)
            {
                logger?.LogError(ex, "An EnterpriseLibraryException occurred.");
                throw; // Rethrow the original exception.
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "An unexpected error occurred.");
                throw new ApplicationException("An error occurred while processing the request.", ex);
            }
        }
    }

}
