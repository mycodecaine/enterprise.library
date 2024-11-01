using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Infrastructure.Authentication.Setting
{
    public class AuthenticationSetting
    {
        public const string DefaultSectionName = "Authentication";
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TokenEndPoint { get; set; }
        public string BaseUrl { get; set; }
        public string RealmName { get; set; }
        public string Admin { get; set; }
        public string Password { get; set; }
    }
}
