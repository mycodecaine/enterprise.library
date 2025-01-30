using Azure;
using Azure.Data.Tables;

namespace Cdcn.Enterprise.Library.Domain.Primitives
{

    /// <summary>
    /// Represents a table entity for Azure Table Storage.
    /// </summary>
    public class TableEntity : ITableEntity
    {
        public TableEntity(string partion)
        {
            PartitionKey = partion;
            Timestamp = DateTime.Now;
            RowKey = Guid.NewGuid().ToString();
            ETag = ETag.All;
        }

        /// <summary>
        /// Gets or sets the partition key for the entity.
        /// </summary>
        public string PartitionKey { get; set; }

        /// <summary>
        /// Gets or sets the row key for the entity.
        /// </summary>
        public string RowKey { get; set; }

        /// <summary>
        /// Gets or sets the timestamp for the entity.
        /// </summary>
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the ETag for the entity.
        /// </summary>
        public ETag ETag { get; set; }

        /// <summary>
        /// Gets the unique identifier for the entity.
        /// </summary>
        public Guid Id => Guid.Parse(RowKey);
    }
}
