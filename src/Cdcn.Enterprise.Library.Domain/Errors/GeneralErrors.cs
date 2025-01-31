using Cdcn.Enterprise.Library.Domain.Primitives.Errors;

namespace Cdcn.Enterprise.Library.Domain.Errors
{
    /// <summary>
    /// Provides a collection of predefined general error messages and factory methods for creating specific types of errors.
    /// This class is static and contains commonly used error definitions for handling unprocessable requests, server errors, and more.
    /// </summary>
    public static class GeneralErrors
    {
        /// <summary>
        /// Represents an error indicating that the server could not process the request.
        /// </summary>        
        public static Error UnProcessableRequest => new Error(
            "General.UnProcessableRequest",
            "The server could not process the request.");

        /// <summary>
        /// Represents an error indicating that the server encountered an unrecoverable error.
        /// </summary>        
        public static Error ServerError => new Error(
            "General.ServerError",
            "The server encountered an unrecoverable error.");

        /// <summary>
        /// Creates an <see cref="EnterpriseLibraryError"/> with the specified error code and exception.
        /// </summary>
        /// <param name="code">The error code associated with the error.</param>
        /// <param name="exception">The exception that caused the error.</param>
        /// <returns>An instance of <see cref="EnterpriseLibraryError"/>.</returns>
        public static EnterpriseLibraryError EnterpriseLibraryError(string code, Exception exception) =>
            new EnterpriseLibraryError(code, exception);

        /// <summary>
        /// Creates a <see cref="ServiceApplicationError"/> with the specified error code and exception.
        /// </summary>
        /// <param name="code">The error code associated with the error.</param>
        /// <param name="exception">The exception that caused the error.</param>
        /// <returns>An instance of <see cref="ServiceApplicationError"/>.</returns>
        public static ServiceApplicationError ServiceApplicationError(string code, Exception exception) =>
            new ServiceApplicationError(code, exception);

        /// <summary>
        /// Creates a <see cref="ServiceInfrastructureError"/> with the specified error code and exception.
        /// </summary>
        /// <param name="code">The error code associated with the error.</param>
        /// <param name="exception">The exception that caused the error.</param>
        /// <returns>An instance of <see cref="ServiceInfrastructureError"/>.</returns>
        public static ServiceInfrastructureError ServiceInfrastructureError(string code, Exception exception) =>
            new ServiceInfrastructureError(code, exception);
    }
}
