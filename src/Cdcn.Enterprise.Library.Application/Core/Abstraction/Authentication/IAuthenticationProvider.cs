using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication.Contracts;
using Cdcn.Enterprise.Library.Domain.Primitives.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication
{
    /// <summary>
    /// Defines the contract for an authentication provider.
    /// </summary>
    public interface IAuthenticationProvider
    {
        /// <summary>
        /// Gets the admin access token.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the admin access token.</returns>
        Task<Result<string>> GetAdminAccessToken();

        /// <summary>
        /// Gets the role ID by name asynchronously.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the role ID.</returns>
        Task<Result<string>> GetRoleIdByNameAsync(string roleName);

        /// <summary>
        /// Logs in a user with the specified username and password.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the token response.</returns>
        Task<Result<string>> Login(string username, string password);

        /// <summary>
        /// Creates a new user with the specified details.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="email">The email address.</param>
        /// <param name="firstName">The first name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="password">The password.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the token response.</returns>
        Task<Result<string>> CreateUser(string username, string email, string firstName, string lastName, string password);

        /// <summary>
        /// Resets the password for the specified user.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <param name="password">The new password.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the password reset was successful.</returns>
        Task<Result<bool>> ResetPassword(string userName, string password);

        /// <summary>
        /// Logs in a user with the specified refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the token response.</returns>
        Task<Result<string>> Login(string refreshToken);

        /// <summary>
        /// Gets the user ID by username.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<Result<string>> GetIdByUserName(string userName);

        /// <summary>
        /// Validate username is exist or not
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<Result<bool>> IsUserNameExist(string userName);
    }
}
