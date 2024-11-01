using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Cdcn.Enterprise.Library.Infrastructure.Authentication
{
    public class UserIdentifierProvider : IUserIdentifierProvider
    {
        public UserIdentifierProvider(IHttpContextAccessor httpContextAccessor)
        {
            var claimPrincipal = httpContextAccessor.GetClaimPrincipal() ??
                throw new ArgumentException("User is not Authenticate", nameof(httpContextAccessor));
            var userIdClaim = claimPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new ArgumentException("User is not Authenticate", nameof(httpContextAccessor));

           

            UserId = new Guid(userIdClaim);
            ClaimsPrincipal = claimPrincipal;
        }
        public Guid UserId { get; }

        public ClaimsPrincipal ClaimsPrincipal { get; }
    }

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
