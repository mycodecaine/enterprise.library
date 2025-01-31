using Cdcn.Enterprise.Library.Domain.Exceptions;

namespace Cdcn.Enterprise.Library.Domain.Primitives.Errors
{

    /// <summary>
    /// Represents an error that occurs in the service infrastructure.
    /// </summary>
    public class ServiceInfrastructureError : Error
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceInfrastructureError"/> class.
        /// </summary>
        /// <param name="code">The error code.</param>
        /// <param name="message">The exception message.</param>
        public ServiceInfrastructureError(string code, Exception message) : base($"Service.Infrastructure.{code}", message.ToJsonString())
        {
        }
    }
}
