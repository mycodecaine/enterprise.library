using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Infrastructure.MessageQueue.AzureQueue.Settings
{
    public class AzureQueueSetting
    {
        public const string DefaultSectionName = "AzureQueue";

        public string ConnectionString { get; set; }
    }
}
