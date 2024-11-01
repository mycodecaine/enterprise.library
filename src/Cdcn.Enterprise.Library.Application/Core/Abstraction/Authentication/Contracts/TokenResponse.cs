using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication.Contracts
{
    public  class TokenResponse
    {
        [JsonConstructor]
        public TokenResponse() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenResponse"/> class.
        /// </summary>
        /// <param name="token">The token value.</param>
        ///  
        public TokenResponse(string token, string refreshToken, DateTime expiredId, DateTime refreshExpiredIn, Guid userId)
        {
            Token = token;
            RefreshToken = refreshToken;
            ExpiredIn = expiredId;
            RefreshExpiredIn = refreshExpiredIn;
            UserId = userId;
        }
        /// <summary>
        /// Gets the token.
        /// </summary>
        public string Token { get; private set; }
        public string RefreshToken { get; private set; }
        public DateTime ExpiredIn { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime RefreshExpiredIn { get; private set; }

    }
}
