using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Authentication;
using System.Security.Claims;

namespace Cdcn.Enterprise.Library.Infrastructure.Authentication
{
    /// <summary>
    /// Represents the implementation of the <see cref="IUserIdentifierProvider"/> interface.
    /// </summary>
    public class UserIdentifierProvider : IUserIdentifierProvider
    {
        public UserIdentifierProvider(IHttpContextAccessor httpContextAccessor)
        {
            var claimPrincipal = httpContextAccessor.GetClaimPrincipal() ??
                throw new AuthenticationException("User is not Authenticate");
            var userIdClaim = claimPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new AuthenticationException("User is not Authenticate");



            UserId = new Guid(userIdClaim);
            ClaimsPrincipal = claimPrincipal;
        }
        public Guid UserId { get; }

        public ClaimsPrincipal ClaimsPrincipal { get; }
    }

    /// <summary>
    /// Represents the mock implementation of the <see cref="IUserIdentifierProvider"/> interface.
    /// NOTES: This class is used for testing purposes only.
    /// </summary>
    public class UserIdentifierProviderMock : IUserIdentifierProvider
    {
        public UserIdentifierProviderMock(IHttpContextAccessor httpContextAccessor)
        {
           

            UserId = Guid.NewGuid();
            
        }
        public Guid UserId { get; }

        public ClaimsPrincipal ClaimsPrincipal { get; }
    }
}
