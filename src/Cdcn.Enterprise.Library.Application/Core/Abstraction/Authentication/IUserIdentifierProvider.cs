using System.Security.Claims;

namespace Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication
{
    public interface IUserIdentifierProvider
    {
        Guid UserId { get; }
        ClaimsPrincipal ClaimsPrincipal { get; }
    }
}