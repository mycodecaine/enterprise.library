using Cdcn.Enterprise.Library.Application.Exceptions;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Tests.Exceptions
{
    internal class ValidationExceptionTests
    {
        [Test]
        public void Constructor_WithValidationFailures_ShouldSetErrorsProperty()
        {
            // Arrange
            var failures = new List<ValidationFailure>
        {
            new ValidationFailure { ErrorCode = "Code1", ErrorMessage = "Error message 1" },
            new ValidationFailure { ErrorCode = "Code2", ErrorMessage = "Error message 2" },
            
        };

            // Act
            var exception = new ValidationException(failures);

            // Assert
            Assert.AreEqual("One or more validation failures has occurred.", exception.Message);
            Assert.NotNull(exception.Errors);
            Assert.AreEqual(2, exception.Errors.Count); // Distinct by ErrorCode and ErrorMessage
            Assert.IsTrue(exception.Errors.Any(e => e.Code == "Code1" && e.Message == "Error message 1"));
            Assert.IsTrue(exception.Errors.Any(e => e.Code == "Code2" && e.Message == "Error message 2"));
        }
    }
}
