using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Infrastructure.Authentication.Helper
{
    public static class JwtHelper
    {
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
