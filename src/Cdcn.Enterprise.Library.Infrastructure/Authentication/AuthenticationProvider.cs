using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication;
using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication.Contracts;
using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication.Events;
using Cdcn.Enterprise.Library.Application.Core.Abstraction.Caching;
using Cdcn.Enterprise.Library.Domain.Primitives.Result;
using Cdcn.Enterprise.Library.Infrastructure.Authentication.Helper;
using Cdcn.Enterprise.Library.Infrastructure.Authentication.Setting;
using Cdcn.Enterprise.Library.Infrastructure.Extension;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace Cdcn.Enterprise.Library.Infrastructure.Authentication
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly AuthenticationSetting _authenticationSetting;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICachingService _cachingService;
        private readonly IMediator _mediator;
        private const string Authencticate = "authenticate-";

        public AuthenticationProvider(IOptions<AuthenticationSetting> authenticationSetting, IHttpClientFactory httpClientFactory, ICachingService cachingService, IMediator mediator)
        {
            _authenticationSetting = authenticationSetting.Value;
            _httpClientFactory = httpClientFactory;
            _cachingService = cachingService;
            _mediator = mediator;
        }

        private async Task<Result<string>> GetAdminAccessToken()
        {
            var key = Authencticate + _authenticationSetting.Admin;
            if (!_cachingService.CacheItemExists(key))
            {
                var data = await Login(_authenticationSetting.Admin, _authenticationSetting.Password);
                _cachingService.SetCacheItem(key, JsonConvert.SerializeObject(data, Formatting.Indented));
                return Result.Success<string>(data.Value.Token);
            }

            var adminTokenJson = _cachingService.GetCacheItem(key);
            var token = JsonConvert.DeserializeObject<TokenResponse>(adminTokenJson);

            if (DateTime.UtcNow < token.ExpiredIn)            {
                
                return Result.Success<string>(token.Token);
            }

            var newToken = await Login(_authenticationSetting.Admin, _authenticationSetting.Password);

            if (newToken.IsFailure) {
                return Result.Failure<string>(AuthenticationErrors.InvalidAdminToken);
            }

            _cachingService.SetCacheItem(key, JsonConvert.SerializeObject(newToken, Formatting.Indented));
            
            return Result.Success<string>(newToken.Value.Token);
        }

        private async Task<Result<string>> GetRoleIdByNameAsync(string roleName)
        {
            var client = _httpClientFactory.CreateClientWithPolicy();
            var userEndpoint = $"{_authenticationSetting.BaseUrl}/admin/realms/{_authenticationSetting.RealmName}/roles/{roleName}";
            var accesstoken = await GetAdminAccessToken();

            if (accesstoken.IsFailure)
            {
                return Result.Failure<string>(AuthenticationErrors.InvalidAdminToken);
            }

            var admintoken = accesstoken.Value;
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", admintoken);

            var response = await client.GetAsync(userEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                return Result.Failure<string>(AuthenticationErrors.InvalidAssignRole);
            }

            var json = await response.Content.ReadAsStringAsync();
            var role = JObject.Parse(json);

            return Result.Success<string>(role["id"].ToString());
        }

        private async Task<Result<TokenResponse>> BaseLogin(string username, string password, string refreshtoken = "")
        {
            var client = _httpClientFactory.CreateClientWithPolicy();

            // Specify the token endpoint URL
            var tokenEndpoint = _authenticationSetting.TokenEndPoint;
            var formData = new List<KeyValuePair<string, string>>();

            formData.Add(new KeyValuePair<string, string>("client_id", _authenticationSetting.ClientId));
            formData.Add(new KeyValuePair<string, string>("client_secret", _authenticationSetting.ClientSecret));
            if (string.IsNullOrEmpty(refreshtoken))
            {
                formData.Add(new KeyValuePair<string, string>("grant_type", "password"));
                formData.Add(new KeyValuePair<string, string>("username", username));
                formData.Add(new KeyValuePair<string, string>("password", password));
            }

            if (!string.IsNullOrEmpty(refreshtoken))
            {
                formData.Add(new KeyValuePair<string, string>("grant_type", "refresh_token"));
                formData.Add(new KeyValuePair<string, string>("refresh_token", refreshtoken));
            }

            var requestContent = new FormUrlEncodedContent(formData);

            // Send a POST request to the token endpoint with the prepared request content
            var response = await client.PostAsync(tokenEndpoint, requestContent);

            // Check if the request was successful
            if (response.IsSuccessStatusCode)
            {
                // Read the access token from the response content
                var token = await response.Content.ReadAsStringAsync();
                JObject content = JObject.Parse(token);
                var expiresIn = int.Parse(content["expires_in"].ToString());
                var refreshexpiresIn = int.Parse(content["refresh_expires_in"].ToString());
                var tokenExpired = DateTime.UtcNow.AddSeconds(expiresIn);
                var refreshtokenExpired = DateTime.UtcNow.AddSeconds(refreshexpiresIn);
                var accessToken = content["access_token"].ToString();
                var sub = JwtHelper.GetSubId(accessToken);
                if(sub == Guid.Empty)
                   return Result.Failure<TokenResponse>(AuthenticationErrors.SubIdNotExist);
                var newtoken = new TokenResponse(accessToken, content["refresh_token"].ToString(), tokenExpired, refreshtokenExpired, sub);
                await _mediator.Publish(new UserLogedInProviderEvent(username, true, ""));
                return Result.Success<TokenResponse>(newtoken);
            }
            await _mediator.Publish(new UserLogedInProviderEvent(username, false, ""));
            return Result.Failure<TokenResponse>(AuthenticationErrors.InvalidUserNameOrPassword);
        }

        public async Task<Result<TokenResponse>> Login(string username, string password)
        {
            return await BaseLogin(username, password);
        }

        public async Task<Result<TokenResponse>> LoginFromRefreshToken(string refreshToken)
        {
            return await BaseLogin("", "", refreshToken);
        }

        public async Task<Result<TokenResponse>> CreateUser(string username, string email, string firstName, string lastName, string password)
        {

            var userNameVerify = await GetIdByUserName(username);
            if (userNameVerify.IsSuccess)
            {
                return Result.Failure<TokenResponse>(AuthenticationErrors.UserNameAlreadyExist);
            }

            var client = _httpClientFactory.CreateClientWithPolicy();
            var userEndpoint = $"{_authenticationSetting.BaseUrl}/admin/realms/{_authenticationSetting.RealmName}/users";
            var  accesstoken = await GetAdminAccessToken();

            if (accesstoken.IsFailure)
            {
                return Result.Failure<TokenResponse>(AuthenticationErrors.InvalidAdminToken);
            }

            var admintoken = accesstoken.Value;

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", admintoken);

            var user = new
            {
                username = username,
                email = email,
                firstName = firstName,
                lastName = lastName,
                enabled = true,
                credentials = new List<object>
            {
                new
                {
                    type = "password",
                    value = password,
                    temporary = false
                }
            }
            };
            var response = await client.PostAsJsonAsync(userEndpoint, user);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                return await Login(username, password);
              
            }

            return Result.Failure<TokenResponse>(AuthenticationErrors.InvalidUserNameOrPassword);
        }



        public async Task<Result<string>> GetIdByUserName(string userName)
        {
            var client = _httpClientFactory.CreateClientWithPolicy();
            var userEndpoint = $"{_authenticationSetting.BaseUrl}/admin/realms/{_authenticationSetting.RealmName}/users?username={userName}";
            var accesstoken = await GetAdminAccessToken();

            if (accesstoken.IsFailure)
            {
                return Result.Failure<string>(AuthenticationErrors.InvalidAdminToken);
            }

            var admintoken = accesstoken.Value;
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", admintoken);

            var response = await client.GetAsync(userEndpoint);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var users = JArray.Parse(responseContent);

            if (users.Count == 1)
            {
                return Result.Success<string>(users[0]["id"].ToString());
            }

            return Result.Failure<string>(AuthenticationErrors.InvalidUserNameOrPassword);
        }

        public async Task<Result<bool>> ResetPassword(string userName, string password)
        {
            var client = _httpClientFactory.CreateClientWithPolicy();
            var userId = await GetIdByUserName(userName.Trim());

            if (userId.IsFailure) {
                return Result.Failure<bool>(AuthenticationErrors.InvalidUserNameOrPassword);
            }
            var userEndpoint = $"{_authenticationSetting.BaseUrl}/admin/realms/{_authenticationSetting.RealmName}/users/{userId}/reset-password";
            
            var accesstoken = await GetAdminAccessToken();

            if (accesstoken.IsFailure)
            {
                return Result.Failure<bool>(AuthenticationErrors.InvalidAdminToken);
            }

            var admintoken = accesstoken.Value;
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", admintoken);

            var requestData = new
            {
                type = "password",
                temporary = false,
                value = password
            };

            var response = await client.PutAsJsonAsync(userEndpoint, requestData);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                return Result.Success<bool>(true);
            }

            return Result.Failure<bool>(AuthenticationErrors.ResetPasswordError);
        }

        public async Task<Result<bool>> AssignRole(string userName, string roleName)
        {
            var client = _httpClientFactory.CreateClientWithPolicy();
            var userId = await GetIdByUserName(userName.Trim());
            if (string.IsNullOrEmpty(userId.Value))
            {
                return false;
            }

            var userEndpoint = $"{_authenticationSetting.BaseUrl}/admin/realms/{_authenticationSetting.RealmName}/users/{userId}/role-mappings/realm";

            var roleId = await GetRoleIdByNameAsync(roleName.Trim());

            if (roleId.IsFailure)
            {
                return Result.Failure<bool>(AuthenticationErrors.InvalidAdminToken);
            }   

            var accesstoken = await GetAdminAccessToken();

            if (accesstoken.IsFailure)
            {
                return Result.Failure<bool>(AuthenticationErrors.InvalidAdminToken);
            }

            var admintoken = accesstoken.Value;
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", admintoken);

            var roleRepresentation = new List<object>
            {
                new
                {
                    id = roleId,
                    name = roleName
                }
            };

            var response = await client.PostAsJsonAsync(userEndpoint, roleRepresentation);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                return Result.Success<bool>(true);
            }

            return Result.Failure<bool>(AuthenticationErrors.AssignRoleNotSuccessfull);
        }
    }
}
