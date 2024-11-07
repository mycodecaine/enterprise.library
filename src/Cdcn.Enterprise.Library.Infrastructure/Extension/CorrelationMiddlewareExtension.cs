using Cdcn.Enterprise.Library.Logging.Correlation;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Infrastructure.Extension
{
    public static class CorrelationMiddlewareExtension
    {
        public static IApplicationBuilder UseCorrelationIdMiddleware(this IApplicationBuilder applicationBuilder)
        => applicationBuilder.UseMiddleware<CorrelationIdMiddleware>();
    }
}
