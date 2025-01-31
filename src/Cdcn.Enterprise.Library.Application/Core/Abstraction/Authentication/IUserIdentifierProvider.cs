using System.Security.Claims;

namespace Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication
{
    /// <summary>
    /// Provides an interface for retrieving user identification information.
    /// </summary>
    public interface IUserIdentifierProvider
    {
        /// <summary>
        /// Gets the unique identifier of the user.
        /// </summary>
        Guid UserId { get; }

        /// <summary>
        /// Gets the claims principal associated with the user.
        /// </summary>
        ClaimsPrincipal ClaimsPrincipal { get; }
    }
}