using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Cdcn.Enterprise.Library.Infrastructure.Authentication
{
    /// <summary>
    /// Provides extension methods for the <see cref="IHttpContextAccessor"/> interface.
    /// </summary>
    public static class UserIdentifierProviderExtension
    {
        /// <summary>
        /// Gets the claims principal associated with the user.
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ClaimsPrincipal GetClaimPrincipal(this IHttpContextAccessor httpContextAccessor)
        {
            var claimPrincipal = httpContextAccessor.HttpContext?.User ??
                throw new ArgumentException("The user identifier claim is required.", nameof(httpContextAccessor));

            return claimPrincipal; //AuthorizationFilterContext
        }

        /// <summary>
        /// Gets the claims principal associated with the user.
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static ClaimsPrincipal GetClaimPrincipal(this AuthorizationFilterContext httpContextAccessor)
        {
            var claimPrincipal = httpContextAccessor.HttpContext?.User ??
                throw new ArgumentException("The user identifier claim is required.", nameof(httpContextAccessor));

            return claimPrincipal; //AuthorizationFilterContext
        }
    }
}
