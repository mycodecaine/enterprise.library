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
        private ILogger<HttpService> _logger;

        public HttpService( IHttpClientFactory httpClientFactory, ILogger<HttpService> logger)
        {           
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, List<KeyValuePair<string, string>> content, string authToken = "")
        {
            var client = _httpClientFactory.CreateClientWithPolicy();
            var requestContent = new FormUrlEncodedContent(content);
            if (!string.IsNullOrEmpty(authToken))
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            // Send a POST request to the token endpoint with the prepared request content
            var response = await client.PostAsync(url, requestContent);
            return response;
        }

        public async Task<HttpResponseMessage> PostJsonAsync<T>(string url, T content, string authToken = "")
        {
            var client = _httpClientFactory.CreateClientWithPolicy();
            if (!string.IsNullOrEmpty(authToken))
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            var response = await client.PostAsJsonAsync(url, content);
            return response;
        }
        public async Task<HttpResponseMessage> GetAsync(string url, string authToken = "")
        {
            var client = _httpClientFactory.CreateClientWithPolicy();
            if (!string.IsNullOrEmpty(authToken))
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);

            // Send a POST request to the token endpoint with the prepared request content
            var response = await client.GetAsync(url);
            return response;
        }

        public async Task<HttpResponseMessage> PutJsonAsync<T>(string url, T content, string authToken = "")
        {
            var client = _httpClientFactory.CreateClientWithPolicy();
            if (!string.IsNullOrEmpty(authToken))
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            var response = await client.PutAsJsonAsync(url, content);
            return response;
        }
    }
}
