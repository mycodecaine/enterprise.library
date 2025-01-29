using Cdcn.Enterprise.Library.Domain.Exceptions;

namespace Cdcn.Enterprise.Library.Domain.Primitives
{
    /// <summary>
    /// Represents an error that occurs in the service application.
    /// </summary>
    public class ServiceApplicationError : Error
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceApplicationError"/> class.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="message">The error message.</param>
        public ServiceApplicationError(string code, Exception message) : base($"Service.Application.{code}", message.ToJsonString())
        {
        }
    }
}