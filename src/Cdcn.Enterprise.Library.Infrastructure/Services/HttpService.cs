using Cdcn.Enterprise.Library.Application.Core.Services;
using Cdcn.Enterprise.Library.Infrastructure.Authentication.Setting;
using Cdcn.Enterprise.Library.Infrastructure.Extension;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace Cdcn.Enterprise.Library.Infrastructure.Services
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HttpService> _logger;

        public HttpService(IHttpClientFactory httpClientFactory, ILogger<HttpService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, List<KeyValuePair<string, string>> content, string authToken = "")
        {
            var client = CreateClient(authToken);
            var requestContent = new FormUrlEncodedContent(content);
            return await client.PostAsync(url, requestContent);
        }

        public async Task<HttpResponseMessage> PostJsonAsync<T>(string url, T content, string authToken = "")
        {
            var client = CreateClient(authToken);
            return await client.PostAsJsonAsync(url, content);
        }

        public async Task<HttpResponseMessage> GetAsync(string url, string authToken = "")
        {
            var client = CreateClient(authToken);
            return await client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PutJsonAsync<T>(string url, T content, string authToken = "")
        {
            var client = CreateClient(authToken);
            return await client.PutAsJsonAsync(url, content);
        }

        /// <summary>
        /// Creates an HttpClient with the specified authorization token.
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        private HttpClient CreateClient(string authToken)
        {
            var client = _httpClientFactory.CreateClientWithPolicy();
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }
            return client;
        }
    }
}
