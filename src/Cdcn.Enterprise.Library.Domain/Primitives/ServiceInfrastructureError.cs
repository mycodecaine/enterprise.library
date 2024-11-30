using Cdcn.Enterprise.Library.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Primitives
{ 
    public class ServiceInfrastructureError : Error
    {
        public ServiceInfrastructureError(string code, Exception message) : base($"Service.Infrastructure.{code}", message.ToJsonString())
        {
        }
    }
}
