using Cdcn.Enterprise.Library.Application.Mediator.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Application.Core.Abstraction.Authentication.Events
{
    public class UserLogedInProviderEvent : IEvent
    {
        public UserLogedInProviderEvent(string userName, bool isAuthenticated, string ip)
        {
            UserName = userName;
            IsAuthenticated = isAuthenticated;
            Ip = ip;
        }

        public string UserName { get; }
        public bool IsAuthenticated { get; }
        public string Ip { get; }
    }
}
