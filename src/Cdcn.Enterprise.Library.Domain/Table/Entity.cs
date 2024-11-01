using Azure;
using Azure.Data.Tables;

namespace Cdcn.Enterprise.Library.Domain.Table
{
    public class Entity : ITableEntity
    {
        public Entity(string partion)
        {
            PartitionKey = partion;
            Timestamp = DateTime.Now;
            RowKey = Guid.NewGuid().ToString();
            ETag = ETag.All;
        }
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        public Guid Id => Guid.Parse(RowKey);
    }
}
