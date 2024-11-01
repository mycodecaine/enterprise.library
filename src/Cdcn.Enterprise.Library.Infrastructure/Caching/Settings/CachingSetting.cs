using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Infrastructure.Caching.Settings
{
    public class CachingSetting
    {
        public const string DefaultSectionName = "Caching";
        public string ConnectionString { get; set; }
    }
}
