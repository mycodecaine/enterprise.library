using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Cdcn.Enterprise.Library.Infrastructure.Extension
{
    public static class HttpClientFactoryExtension
    {
        public const string MyClientWithPolicy = "MyClientWithPolicy";

        public static HttpClient CreateClientWithPolicy(this IHttpClientFactory factory)
        {
            return factory.CreateClient(MyClientWithPolicy);
        }

        public static IServiceCollection AddHttpClientWithPolicy(this IServiceCollection services)
        {
            services.AddHttpClient(MyClientWithPolicy).AddPolicyHandler(GetRetryPolicy()).
                AddPolicyHandler(GetCircuitBreakerPolicy()).
                AddPolicyHandler(GetTimeoutPolicy());

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
        {
            return Policy.TimeoutAsync<HttpResponseMessage>(10); // Timeout after 10 seconds
        }
    }
}
