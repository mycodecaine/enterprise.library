using Cdcn.Enterprise.Library.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace Cdcn.Enterprise.Library.Domain.Exceptions
{
    public sealed class ExceptionHelper
    {
        public static EnterpriseLibraryException EnterpriseLibraryException(Exception exception, ILogger logger, string context)
        {
            var error = GeneralErrors.EnterpriseLibraryError(context, exception);
            logger.LogCritical(error.Message);
            return new EnterpriseLibraryException(error);
        }

        public static ServiceInfrastructureException ServiceInfrastructureException(Exception exception, ILogger logger, string context)
        {
            var error = GeneralErrors.ServiceInfrastructureError(context, exception);
            logger.LogCritical(error.Message);
            return new ServiceInfrastructureException(error);
        }

        public static ServiceApplicationException ServiceApplicationException(Exception exception, ILogger logger, string context)
        {
            var error = GeneralErrors.ServiceApplicationError(context, exception);
            logger.LogCritical(error.Message);
            return new ServiceApplicationException(error);
        }
    }
}
