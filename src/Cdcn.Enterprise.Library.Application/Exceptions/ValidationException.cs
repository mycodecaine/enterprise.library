using Cdcn.Enterprise.Library.Domain.Primitives.Errors;
using FluentValidation.Results;

namespace Cdcn.Enterprise.Library.Application.Exceptions
{
    /// <summary>
    /// Represents an exception that occurs during validation.
    /// </summary>
    public sealed class ValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="failures">The collection of validation failures.</param>
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : base("One or more validation failures has occurred.") =>
            Errors = failures
                .Distinct()
                .Select(failure => new Error(failure.ErrorCode, failure.ErrorMessage))
                .ToList();

        public ValidationException(Error error)
           : base(error.Message)
           => Error = error;

        /// <summary>
        /// Gets the error.
        /// </summary>
        public Error Error { get; }

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        public IReadOnlyCollection<Error> Errors { get; }
    }
}
