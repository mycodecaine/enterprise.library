using Cdcn.Enterprise.Library.Domain.Primitives.Errors;

namespace Cdcn.Enterprise.Library.Domain.Exceptions
{

    /// <summary>
    /// Represents an exception specific to the Enterprise Library.
    /// </summary>
    public class EnterpriseLibraryException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="error">The error containing the information about what happened.</param>
        public EnterpriseLibraryException(EnterpriseLibraryError error)
            : base(error.Message)
            => Error = error;

        /// <summary>
        /// Gets the error.
        /// </summary>
        public Error Error { get; }
    }
}
