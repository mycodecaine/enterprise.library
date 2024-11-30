using Cdcn.Enterprise.Library.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Primitives
{
   

    public class ServiceApplicationError : Error
    {
        public ServiceApplicationError(string code, Exception message) : base($"Service.Application.{code}", message.ToJsonString())
        {
        }
    }
}
