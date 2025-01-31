using Cdcn.Enterprise.Library.Domain.Primitives.Errors;

namespace Cdcn.Enterprise.Library.Domain.Exceptions
{
    /// <summary>
    /// Represents an exception that occurs in the service application.
    /// </summary>
    public class ServiceApplicationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainException"/> class.
        /// </summary>
        /// <param name="error">The error containing the information about what happened.</param>
        public ServiceApplicationException(ServiceApplicationError error)
            : base(error.Message)
            => Error = error;

        /// <summary>
        /// Gets the error.
        /// </summary>
        public Error Error { get; }
    }
}
