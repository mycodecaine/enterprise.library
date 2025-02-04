using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Core.Services
{
    /// <summary>
    /// Interface for HTTP service to handle HTTP requests.
    /// </summary>
    public interface IHttpService
    {
        /// <summary>
        /// Sends a POST request with JSON content  with an optional authorization token.
        /// </summary>
        /// <typeparam name="T">Type of the content to be sent.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="content">The content to be sent in the request body.</param>
        /// <param name="authToken">The authorization token to be included in the request headers.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
        Task<HttpResponseMessage> PostJsonAsync<T>(string url, T content, string authToken = "");

        /// <summary>
        /// Sends a POST request with form-urlencoded content  with an optional authorization token.
        /// </summary>
        /// <typeparam name="T">Type of the content to be sent.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="content">The content to be sent in the request body as a list of key-value pairs.</param>
        /// <param name="authToken">The authorization token to be included in the request headers.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
        Task<HttpResponseMessage> PostAsync(string url, List<KeyValuePair<string, string>> content, string authToken = "" );

        /// <summary>
        /// Sends a GET request with an optional authorization token.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="authToken">The authorization token to be included in the request headers.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
        Task<HttpResponseMessage> GetAsync(string url, string authToken = "");

        /// <summary>
        /// Sends a PUT request with JSON content  with an optional authorization token.
        /// </summary>
        /// <typeparam name="T">Type of the content to be sent.</typeparam>
        /// <param name="url">The URL to send the request to.</param>
        /// <param name="content">The content to be sent in the request body.</param>
        /// <param name="authToken">The authorization token to be included in the request headers.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the HTTP response message.</returns>
        Task<HttpResponseMessage> PutJsonAsync<T>(string url, T content, string authToken = "");
    }
}
