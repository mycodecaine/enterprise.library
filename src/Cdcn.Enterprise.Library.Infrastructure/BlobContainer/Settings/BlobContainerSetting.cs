namespace Cdcn.Enterprise.Library.Infrastructure.BlobContainer.Settings
{
    public class BlobContainerSetting
    {
        public const string DefaultSectionName = "BlobContainer";
        public const string DefaultContainerName = "swq";
        public string ConnectionString { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageKey { get; set; }
    }
}