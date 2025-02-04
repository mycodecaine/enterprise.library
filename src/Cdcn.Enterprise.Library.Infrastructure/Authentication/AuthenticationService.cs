using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication;
using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication.Contracts;
using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication.Events;
using Cdcn.Enterprise.Library.Application.Core.Abstraction.Caching;
using Cdcn.Enterprise.Library.Domain.Exceptions;
using Cdcn.Enterprise.Library.Domain.Primitives.Result;
using Cdcn.Enterprise.Library.Infrastructure.Authentication.Helper;
using Cdcn.Enterprise.Library.Infrastructure.Authentication.Setting;
using Cdcn.Enterprise.Library.Infrastructure.Extension;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly.Timeout;
using System.Net.Http.Json;

namespace Cdcn.Enterprise.Library.Infrastructure.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        
        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly ICacheService _cachingService;
        private readonly IMediator _mediator;
        private const string Authencticate = "authenticate-admin";
        private ILogger<AuthenticationService> _logger;

        public AuthenticationService( IAuthenticationProvider authenticationProvider, ICacheService cachingService, IMediator mediator, ILogger<AuthenticationService> logger)
        {
           
            _authenticationProvider = authenticationProvider;
            _cachingService = cachingService;
            _mediator = mediator;
            _logger = logger;
        }
        /// <summary>
        /// Get the admin access token
        /// </summary>
        /// <returns>Token</returns>
        protected async Task<Result<string>> GetAdminAccessToken()
        {
            var key = Authencticate;
            try
            {
                if (!_cachingService.CacheItemExists(key))
                {
                    var data = await _authenticationProvider.GetAdminAccessToken();
                    if ( data.IsFailure)
                    {
                        return Result.Failure<string>(AuthenticationErrors.InvalidAdminToken);
                    }
                    _cachingService.SetCacheItem(key, data.Value);
                    var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(data.Value);
                    return Result.Success<string>(tokenResponse.Token);
                }

                var adminTokenJson = _cachingService.GetCacheItem(key);
                var token = JsonConvert.DeserializeObject<TokenResponse>(adminTokenJson);

                if (DateTime.UtcNow < token.ExpiredIn)
                {

                    return Result.Success<string>(token.Token);
                }

                var newAdminToken = await _authenticationProvider.GetAdminAccessToken();

                if (newAdminToken.IsFailure)
                {
                    return Result.Failure<string>(AuthenticationErrors.InvalidAdminToken);
                }

                _cachingService.SetCacheItem(key, newAdminToken.Value);
                var newtoken = JsonConvert.DeserializeObject<TokenResponse>(newAdminToken.Value);
                return Result.Success<string>(newtoken.Token);
            }
            catch (TimeoutRejectedException ex)
            {
                throw ExceptionHelper.EnterpriseLibraryException(ex, _logger, $"{typeof(AuthenticationService).FullName}.GetAdminAccessToken.TimeOut");
            }
            catch (Exception ex)
            {
                throw ExceptionHelper.EnterpriseLibraryException(ex, _logger, $"{typeof(AuthenticationService).FullName}.GetAdminAccessToken");

            }
        }
        /// <summary>
        /// Get the role id by role name
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        protected async Task<Result<string>> GetRoleIdByNameAsync(string roleName)
        {               

            return await _authenticationProvider.GetRoleIdByNameAsync(roleName);
        }
        /// <summary>
        /// Base login method
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="refreshtoken"></param>
        /// <returns></returns>
        virtual protected async Task<Result<TokenResponse>> BaseLogin(string username, string password, string refreshtoken = "")
        {

            var token = "";
            if (string.IsNullOrEmpty(refreshtoken))
            {
                var loginWithUserName = await _authenticationProvider.Login(username, password);

                if (loginWithUserName.IsFailure)
                {
                    await _mediator.Publish(new UserLogedInProviderEvent(username, false, ""));
                    return Result.Failure<TokenResponse>(AuthenticationErrors.InvalidUserNameOrPassword);
                }
                token = loginWithUserName.Value;
            }

            if (!string.IsNullOrEmpty(refreshtoken))
            {
                var loginWithRefreshToken = await _authenticationProvider.Login(refreshtoken);
                if (loginWithRefreshToken.IsFailure)
                {
                    await _mediator.Publish(new UserLogedInProviderEvent(username, false, ""));
                    return Result.Failure<TokenResponse>(AuthenticationErrors.InvalidUserNameOrPassword);
                }
                token = loginWithRefreshToken.Value;

            }

            JObject content = JObject.Parse(token);
            var expiresIn = int.Parse(content["expires_in"].ToString());
            var refreshexpiresIn = int.Parse(content["refresh_expires_in"].ToString());
            var tokenExpired = DateTime.UtcNow.AddSeconds(expiresIn);
            var refreshtokenExpired = DateTime.UtcNow.AddSeconds(refreshexpiresIn);
            var accessToken = content["access_token"].ToString();
            var sub = JwtHelper.GetSubId(accessToken);
            if (sub == Guid.Empty)
            {
                await _mediator.Publish(new UserLogedInProviderEvent(username, false, ""));
                return Result.Failure<TokenResponse>(AuthenticationErrors.SubIdNotExist);                
            }
            var newtoken = new TokenResponse(accessToken, content["refresh_token"].ToString(), tokenExpired, refreshtokenExpired, sub);
            await _mediator.Publish(new UserLogedInProviderEvent(username, true, ""));
            return Result.Success<TokenResponse>(newtoken);
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

            var userNameVerify = await _authenticationProvider.IsUserNameExist(username);
            if (userNameVerify.IsSuccess && userNameVerify.Value)
            {
                return Result.Failure<TokenResponse>(AuthenticationErrors.UserNameAlreadyExist);
            }            
            var  accesstoken = await GetAdminAccessToken();
            if (accesstoken.IsFailure)
            {
                return Result.Failure<TokenResponse>(AuthenticationErrors.InvalidAdminToken);
            }
            var admintoken = accesstoken.Value;
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
            var response = await _authenticationProvider.CreateUser(username, email, firstName, lastName, password);
            
            if (response.IsSuccess)
            {
                return await Login(username, password);
              
            }
            return Result.Failure<TokenResponse>(AuthenticationErrors.InvalidUserNameOrPassword);
        }
        public async Task<Result<string>> GetIdByUserName(string userName)
        {           

            try
            {
               var response = await _authenticationProvider.GetIdByUserName(userName);
               return response;
            }
            catch (TimeoutRejectedException ex)
            {
                throw ExceptionHelper.EnterpriseLibraryException(ex, _logger, $"{typeof(AuthenticationService).FullName}.GetIdByUserName.TimeOut");
            }

            catch (Exception ex)
            {
                throw ExceptionHelper.EnterpriseLibraryException(ex, _logger, $"{typeof(AuthenticationService).FullName}.GetIdByUserName");
            }
        }
        public async Task<Result<bool>> ResetPassword(string userName, string password)
        {


            var requestData = new
            {
                type = "password",
                temporary = false,
                value = password
            };

            var response = await _authenticationProvider.ResetPassword(userName, password);
            return response;
        }
        //public async Task<Result<bool>> AssignRole(string userName, string roleName)
        //{
        //    var client = _authenticationProvider.CreateClientWithPolicy();
        //    var userId = await GetIdByUserName(userName.Trim());
        //    if (string.IsNullOrEmpty(userId.Value))
        //    {
        //        return false;
        //    }

        //    var userEndpoint = $"{_authenticationSetting.BaseUrl}/admin/realms/{_authenticationSetting.RealmName}/users/{userId}/role-mappings/realm";

        //    var roleId = await GetRoleIdByNameAsync(roleName.Trim());

        //    if (roleId.IsFailure)
        //    {
        //        return Result.Failure<bool>(AuthenticationErrors.InvalidAdminToken);
        //    }   

        //    var accesstoken = await GetAdminAccessToken();

        //    if (accesstoken.IsFailure)
        //    {
        //        return Result.Failure<bool>(AuthenticationErrors.InvalidAdminToken);
        //    }

        //    var admintoken = accesstoken.Value;
        //    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", admintoken);

        //    var roleRepresentation = new List<object>
        //    {
        //        new
        //        {
        //            id = roleId,
        //            name = roleName
        //        }
        //    };

        //    var response = await client.PostAsJsonAsync(userEndpoint, roleRepresentation);
        //    response.EnsureSuccessStatusCode();
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return Result.Success<bool>(true);
        //    }

        //    return Result.Failure<bool>(AuthenticationErrors.AssignRoleNotSuccessfull);
        //}
    }
}
