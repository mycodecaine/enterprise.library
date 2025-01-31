namespace Cdcn.Enterprise.Library.Domain.Common
{
    /// <summary>  
    /// Provides an interface for accessing the current UTC date and time.  
    /// </summary>  
    public interface IDateTime
    {
        /// <summary>  
        /// Gets the current UTC date and time.  
        /// </summary>  
        DateTime UtcNow { get; }
    }
}
