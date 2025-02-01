using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication;
using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication.Contracts;
using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication.Events;
using Cdcn.Enterprise.Library.Application.Core.Abstraction.Caching;
using Cdcn.Enterprise.Library.Domain.Primitives.Result;
using Cdcn.Enterprise.Library.Infrastructure.Authentication.Helper;
using Cdcn.Enterprise.Library.Infrastructure.Authentication.Setting;
using Cdcn.Enterprise.Library.Infrastructure.Extension;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Cdcn.Enterprise.Library.Infrastructure.Authentication
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly AuthenticationSetting _authenticationSetting;
        private readonly IHttpClientFactory _httpClientFactory; 
        private ILogger<AuthenticationProvider> _logger;

        private const string Authenticate = "authenticate-";

        public AuthenticationProvider(AuthenticationSetting authenticationSetting, IHttpClientFactory httpClientFactory, ILogger<AuthenticationProvider> logger)
        {
            _authenticationSetting = authenticationSetting;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public Task<Result<string>> CreateUser(string username, string email, string firstName, string lastName, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<string>> GetAdminAccessToken()
        {
            var adminToken = await Login(_authenticationSetting.Admin,_authenticationSetting.Password);
            return adminToken;
        }

        public Task<Result<string>> GetRoleIdByNameAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<string>> Login(string username, string password)
        {
            var client = _httpClientFactory.CreateClientWithPolicy();

            // Specify the token endpoint URL
            var tokenEndpoint = _authenticationSetting.TokenEndPoint;
            var formData = new List<KeyValuePair<string, string>>();

            formData.Add(new KeyValuePair<string, string>("client_id", _authenticationSetting.ClientId));
            formData.Add(new KeyValuePair<string, string>("client_secret", _authenticationSetting.ClientSecret));

            formData.Add(new KeyValuePair<string, string>("grant_type", "password"));
            formData.Add(new KeyValuePair<string, string>("username", username));
            formData.Add(new KeyValuePair<string, string>("password", password));



            var requestContent = new FormUrlEncodedContent(formData);

            // Send a POST request to the token endpoint with the prepared request content
            var response = await client.PostAsync(tokenEndpoint, requestContent);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Read the access token from the response content
                var token = await response.Content.ReadAsStringAsync();
                

                return Result.Success<string>(token);
            }

            return Result.Failure<string>(AuthenticationErrors.InvalidUserNameOrPassword);
        }

        public Task<Result<string>> Login(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<bool>> ResetPassword(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
