using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Cdcn.Enterprise.Library.Infrastructure.Authentication
{
    public static class UserIdentifierProviderExtension
    {
        public static ClaimsPrincipal GetClaimPrincipal(this IHttpContextAccessor httpContextAccessor)
        {
            var claimPrincipal = httpContextAccessor.HttpContext?.User ??
                throw new ArgumentException("The user identifier claim is required.", nameof(httpContextAccessor));

            return claimPrincipal; //AuthorizationFilterContext
        }

        public static ClaimsPrincipal GetClaimPrincipal(this AuthorizationFilterContext httpContextAccessor)
        {
            var claimPrincipal = httpContextAccessor.HttpContext?.User ??
                throw new ArgumentException("The user identifier claim is required.", nameof(httpContextAccessor));

            return claimPrincipal; //AuthorizationFilterContext
        }
    }
}
