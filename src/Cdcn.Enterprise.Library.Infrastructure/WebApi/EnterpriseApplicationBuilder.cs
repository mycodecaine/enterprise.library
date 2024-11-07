using Cdcn.Enterprise.Library.Infrastructure.Extension;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Infrastructure.WebApi
{
    public static class EnterpriseApplicationBuilder
    {
        public static IApplicationBuilder UseEnterpriseApplicationBuilder(this IApplicationBuilder app)
        {
            app.UseCorrelationIdMiddleware();
            app.UseCompression();
            app.UseCustomExceptionHandler();
            return app;
        }
    }
}
