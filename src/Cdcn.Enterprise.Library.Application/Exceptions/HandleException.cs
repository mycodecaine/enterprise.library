using Cdcn.Enterprise.Library.Domain.Errors;
using Cdcn.Enterprise.Library.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Exceptions
{
    public static class HandleException
    {

        public static ServiceApplicationException ThrowServiceApplicationException(this Exception exception, ILogger logger, string context)
        {
            var error = GeneralErrors.ServiceApplicationError(context, exception);
            logger.LogCritical(error.Message);
            return new ServiceApplicationException(error);
        }
    }
}
