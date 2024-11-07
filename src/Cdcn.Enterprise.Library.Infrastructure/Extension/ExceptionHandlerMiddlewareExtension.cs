using Cdcn.Enterprise.Library.Infrastructure.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Cdcn.Enterprise.Library.Infrastructure.Extension
{
    public static class ExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<ExceptionHandlerMiddleware>();
    }
}
