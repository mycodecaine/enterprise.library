using Cdcn.Enterprise.Library.Domain.Errors;
using Cdcn.Enterprise.Library.Domain.Exceptions;
using Cdcn.Enterprise.Library.Domain.Primitives;
using Cdcn.Enterprise.Library.Infrastructure.Authentication;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Infrastructure.Exceptions
{
    public static class HandleException
    {

        public static EnterpriseLibraryException ThrowEnterpriseLibraryException(this Exception exception, ILogger logger, string context)
        {
            var error = GeneralErrors.EnterpriseLibraryError(context, exception);
            logger.LogCritical(error.Message);
            return new EnterpriseLibraryException(error);
        }

        public static ServiceApplicationException ThrowServiceApplicationException(this Exception exception, ILogger logger, string context)
        {
            var error = GeneralErrors.ServiceApplicationError(context, exception);
            logger.LogCritical(error.Message);
            return new ServiceApplicationException(error);
        }

        public static ServiceInfrastructureException ThrowServiceInfrastructureException(this Exception exception, ILogger logger, string context)
        {
            var error = GeneralErrors.ServiceInfrastructureError(context, exception);
            logger.LogCritical(error.Message);
            return new ServiceInfrastructureException(error);
        }
    }
}
