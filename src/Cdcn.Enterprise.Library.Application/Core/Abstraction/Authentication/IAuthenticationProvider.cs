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
    /// Provides methods for user authentication and management.
    /// </summary>
    public interface IAuthenticationProvider
    {
        /// <summary>
        /// Authenticates a user with the specified username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{TokenResponse}"/> with the authentication token.</returns>
        Task<Result<TokenResponse>> Login(string username, string password);

        /// <summary>
        /// Authenticates a user using a refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{TokenResponse}"/> with the new authentication token.</returns>
        Task<Result<TokenResponse>> LoginFromRefreshToken(string refreshToken);

        /// <summary>
        /// Creates a new user with the specified details.
        /// </summary>
        /// <param name="username">The username of the new user.</param>
        /// <param name="email">The email of the new user.</param>
        /// <param name="firstName">The first name of the new user.</param>
        /// <param name="lastName">The last name of the new user.</param>
        /// <param name="password">The password of the new user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{TokenResponse}"/> with the authentication token for the new user.</returns>
        Task<Result<TokenResponse>> CreateUser(string username, string email, string firstName, string lastName, string password);

        /// <summary>
        /// Gets the user ID by the specified username.
        /// </summary>
        /// <param name="userName">The username of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{string}"/> with the user ID.</returns>
        Task<Result<string>> GetIdByUserName(string userName);

        /// <summary>
        /// Resets the password for the specified user.
        /// </summary>
        /// <param name="userName">The username of the user.</param>
        /// <param name="password">The new password for the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{bool}"/> indicating whether the password reset was successful.</returns>
        Task<Result<bool>> ResetPassword(string userName, string password);

        /// <summary>
        /// Assigns a role to the specified user.
        /// </summary>
        /// <param name="userName">The username of the user.</param>
        /// <param name="roleName">The name of the role to assign.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Result{bool}"/> indicating whether the role assignment was successful.</returns>
        Task<Result<bool>> AssignRole(string userName, string roleName);
    }
}
