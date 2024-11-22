using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Primitives
{
    public class EnterpriseLibraryError : Error
    {
        public EnterpriseLibraryError(string code, string message) : base($"EnterpriseLibrary-{code}", message)
        {
        }
    }
}
