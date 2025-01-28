namespace Cdcn.Enterprise.Library.Domain.Data
{
    /// <summary>
    /// Interface to enable entity auditing.
    /// </summary>
    public interface IAuditableEntity
    {
        /// <summary>
        /// Gets the created on date and time in UTC format.
        /// </summary>
        DateTime CreatedOnUtc { get; }

        /// <summary>
        /// Gets the modified on date and time in UTC format.
        /// </summary>
        DateTime? ModifiedOnUtc { get; }

        /// <summary>
        /// Created By
        /// </summary>
        Guid? CreatedBy { get; }

        /// <summary>
        /// ModifiedBy
        /// </summary>
        Guid? ModifiedBy { get; }

    }
}
