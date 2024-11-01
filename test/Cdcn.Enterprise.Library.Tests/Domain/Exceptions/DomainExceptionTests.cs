using Cdcn.Enterprise.Library.Domain.Exceptions;
using Cdcn.Enterprise.Library.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Tests.Exceptions
{
    internal class DomainExceptionTests
    {
        [Test]
        public void Constructor_ShouldSetErrorProperty()
        {
            // Arrange
            var error = new Error("Code1", "An error occurred.");

            // Act
            var exception = new DomainException(error);

            // Assert
            Assert.AreEqual(error, exception.Error);
        }

        [Test]
        public void Constructor_ShouldSetMessageToErrorMessage()
        {
            // Arrange
            var error = new Error("Code1", "An error occurred.");

            // Act
            var exception = new DomainException(error);

            // Assert
            Assert.AreEqual("An error occurred.", exception.Message);
        }
    }
}
