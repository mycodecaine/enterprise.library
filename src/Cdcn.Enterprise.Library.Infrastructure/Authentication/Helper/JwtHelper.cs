using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Infrastructure.Authentication.Helper
{
    /// <summary>
    /// Provides helper methods for working with JSON Web Tokens (JWT).
    /// </summary>
    public static class JwtHelper
    {
        /// <summary>
        /// Extracts the Subject (sub) claim from a JWT and returns it as a Guid.
        /// </summary>
        /// <param name="token">The JWT from which to extract the Subject claim.</param>
        /// <returns>The Subject claim as a Guid, or Guid.Empty if the claim is not present or invalid.</returns>
        public static Guid GetSubId(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var value = handler.ReadJwtToken(token);

            var id = Guid.TryParse(value.Subject, out Guid subId);

            if (!id)
                return Guid.Empty;

            return subId;
        }
    }
}
