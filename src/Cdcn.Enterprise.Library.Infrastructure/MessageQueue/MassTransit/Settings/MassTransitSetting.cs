using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Infrastructure.MessageQueue.MassTransit.Settings
{
    public class MassTransitSetting
    {
        public const string DefaultSectionName = "MassTransit";

        public string ConnectionString { get; set; }
    }
}
