using Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication.Contracts;
using Cdcn.Enterprise.Library.Domain.Primitives.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication
{
    public interface IAuthenticationProvider
    {
        Task<Result<TokenResponse>> Login(string username, string password);

        Task<Result<TokenResponse>> LoginFromRefreshToken(string refreshToken);

        Task<Result<TokenResponse>> CreateUser(string username, string email, string firstName, string lastName, string password);

        Task<Result<string>> GetIdByUserName(string userName);

        Task<Result<bool>> ResetPassword(string userName, string password);

        Task<Result<bool>> AssignRole(string userName, string roleName);
    }
}
