using Cdcn.Enterprise.Library.Infrastructure.Extension;
using Microsoft.Extensions.DependencyInjection;

namespace Cdcn.Enterprise.Library.Infrastructure.WebApi
{
    public static class EnterpriseServiceCollection
    {
        public static IServiceCollection AddEnterpriseServiceCollection(this IServiceCollection services)
        {
            services.AddCompression();
            services.AddHttpClientWithPolicy();
            services.AddJwtSwaggerGen();

            return services;
        }
    }
}
