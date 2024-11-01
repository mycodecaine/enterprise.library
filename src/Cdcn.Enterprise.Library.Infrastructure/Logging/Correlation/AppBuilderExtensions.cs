using Microsoft.AspNetCore.Builder;

namespace Cdcn.Enterprise.Library.Logging.Correlation;

public static class AppBuilderExtensions
{
    public static IApplicationBuilder AddCorrelationIdMiddleware(this IApplicationBuilder applicationBuilder)
        => applicationBuilder.UseMiddleware<CorrelationIdMiddleware>();
}