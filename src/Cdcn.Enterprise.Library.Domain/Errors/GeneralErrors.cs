using Cdcn.Enterprise.Library.Domain.Exceptions;
using Cdcn.Enterprise.Library.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Errors
{
    public static class GeneralErrors
    {
        public static Error UnProcessableRequest => new Error(
              "General.UnProcessableRequest",
              "The server could not process the request.");

        public static Error ServerError => new Error("General.ServerError", "The server encountered an unrecoverable error.");

        public static EnterpriseLibraryError EnterpriseLibraryError(string code, Exception exception) => new EnterpriseLibraryError(code, exception);
        public static ServiceApplicationError ServiceApplicationError(string code, Exception exception) => new ServiceApplicationError(code, exception);
        public static ServiceInfrastructureError ServiceInfrastructureError(string code, Exception exception) => new ServiceInfrastructureError(code, exception);

    }
}
