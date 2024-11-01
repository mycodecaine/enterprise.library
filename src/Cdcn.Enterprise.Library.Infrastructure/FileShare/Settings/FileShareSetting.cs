using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Infrastructure.FileShare.Settings
{
    public class FileShareSetting
    {
        public const string DefaultSectionName = "FileShare";
        public const string DefaultDirectoryName = "swq";
        public string ConnectionString { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageKey { get; set; }
    }
}
