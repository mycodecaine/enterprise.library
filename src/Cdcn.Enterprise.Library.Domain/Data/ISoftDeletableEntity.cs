namespace Cdcn.Enterprise.Library.Domain.Data
{
    /// <summary>
    /// Interface to enable entity to be soft deleting.
    /// Notes : https://chatgpt.com/share/6799694a-3bfc-8007-b1f0-dff7fa32aba4
    /// </summary>
    public interface ISoftDeletableEntity
    {
        /// <summary>
        /// Gets the date and time in UTC format the entity was deleted on.        
        /// </summary>
        DateTime? DeletedOnUtc { get; }

        /// <summary>
        /// Gets a value indicating whether the entity has been deleted.
        /// </summary>
        bool Deleted { get; }
    }
}
