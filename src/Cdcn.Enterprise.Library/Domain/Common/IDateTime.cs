using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Common
{
    public interface IDateTime
    {
        DateTime UtcNow { get; }
    }
}
