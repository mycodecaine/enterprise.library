using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Infrastructure.Authentication.Setting
{
    /// <summary>
    /// Represents the settings required for authentication.
    /// </summary>
    public class AuthenticationSetting
    {
        /// <summary>
        /// The default section name for authentication settings.
        /// </summary>
        public const string DefaultSectionName = "Authentication";

        /// <summary>
        /// Gets or sets the client ID for authentication.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret for authentication.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the token endpoint URL.
        /// </summary>
        public string TokenEndPoint { get; set; }

        /// <summary>
        /// Gets or sets the base URL for authentication.
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Gets or sets the realm name for authentication.
        /// </summary>
        public string RealmName { get; set; }

        /// <summary>
        /// Gets or sets the admin username for authentication.
        /// </summary>
        public string Admin { get; set; }

        /// <summary>
        /// Gets or sets the password for authentication.
        /// </summary>
        public string Password { get; set; }
    }
}
