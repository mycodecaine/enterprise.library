using Cdcn.Enterprise.Library.Domain.Exceptions;

namespace Cdcn.Enterprise.Library.Domain.Primitives
{
    /// <summary>
    /// Represents an error specific to the Enterprise Library.
    /// </summary>
    public class EnterpriseLibraryError : Error
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseLibraryError"/> class.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="message">The error message.</param>
        public EnterpriseLibraryError(string code, Exception message) : base($"EnterpriseLibrary.{code}", message.ToJsonString())
        {
        }
    }
}
