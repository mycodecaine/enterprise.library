using Cdcn.Enterprise.Library.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace Cdcn.Enterprise.Library.Domain.Exceptions
{
    /// <summary>
    /// Provides helper methods for creating and logging exceptions.
    /// </summary>
    public sealed class ExceptionHelper
    {
        /// <summary>
        /// Creates an <see cref="EnterpriseLibraryException"/> and logs the error.
        /// </summary>
        /// <param name="exception">The original exception.</param>
        /// <param name="logger">The logger to use for logging the error.</param>
        /// <param name="context">The context in which the error occurred.</param>
        /// <returns>An <see cref="EnterpriseLibraryException"/> instance.</returns>
        public static EnterpriseLibraryException EnterpriseLibraryException(Exception exception, ILogger logger, string context)
        {
            var error = GeneralErrors.EnterpriseLibraryError(context, exception);
            logger.LogCritical(error.Message);
            return new EnterpriseLibraryException(error);
        }

        /// <summary>
        /// Creates a <see cref="ServiceInfrastructureException"/> and logs the error.
        /// </summary>
        /// <param name="exception">The original exception.</param>
        /// <param name="logger">The logger to use for logging the error.</param>
        /// <param name="context">The context in which the error occurred.</param>
        /// <returns>A <see cref="ServiceInfrastructureException"/> instance.</returns>
        public static ServiceInfrastructureException ServiceInfrastructureException(Exception exception, ILogger logger, string context)
        {
            var error = GeneralErrors.ServiceInfrastructureError(context, exception);
            logger.LogCritical(error.Message);
            return new ServiceInfrastructureException(error);
        }

        /// <summary>
        /// Creates a <see cref="ServiceApplicationException"/> and logs the error.
        /// </summary>
        /// <param name="exception">The original exception.</param>
        /// <param name="logger">The logger to use for logging the error.</param>
        /// <param name="context">The context in which the error occurred.</param>
        /// <returns>A <see cref="ServiceApplicationException"/> instance.</returns>
        public static ServiceApplicationException ServiceApplicationException(Exception exception, ILogger logger, string context)
        {
            var error = GeneralErrors.ServiceApplicationError(context, exception);
            logger.LogCritical(error.Message);
            return new ServiceApplicationException(error);
        }
    }
}
