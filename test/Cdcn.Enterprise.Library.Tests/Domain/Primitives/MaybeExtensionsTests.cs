using Cdcn.Enterprise.Library.Domain.Primitives;
using Cdcn.Enterprise.Library.Domain.Primitives.Maybe;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Tests.Primitives.Maybe
{
    [TestFixture]
    public class MaybeExtensionsTests
    {
        [Test]
        public async Task Bind_WithValue_ReturnsBoundValue()
        {
            // Arrange
            var maybe = Maybe<int>.From(5);
            Func<int, Task<Maybe<string>>> func = i => Task.FromResult(Maybe<string>.From(i.ToString()));

            // Act
            var result = await maybe.Bind(func);

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual("5", result.Value);
        }

        [Test]
        public async Task Bind_WithoutValue_ReturnsNone()
        {
            // Arrange
            var maybe = Maybe<int>.None;

            // Act
            var result = maybe.Error;

            // Assert
            Assert.That(result.Code,Is.EqualTo("None"));
        }

        [Test]
        public async Task Match_WithValue_ReturnsOnSuccessResult()
        {
            // Arrange
            var maybe = Task.FromResult(Maybe<int>.From(5));
            Func<int, string> onSuccess = i => i.ToString();
            Func<Error, string> onFailure = e => e.Message;

            // Act
            var result = await maybe.Match(onSuccess, onFailure);

            // Assert
            Assert.AreEqual("5", result);
        }

       
        public async Task Match_WithoutValue_ReturnsOnFailureResult()
        {
            // Arrange
            var error = new Error("404", "Not Found");
            var maybe = Task.FromResult(Maybe<int>.Exception(error));
            Func<int, string> onSuccess = i => i.ToString();
            Func<Error, string> onFailure = e => e.Message;

            // Act
            var result = await maybe.Match(onSuccess, onFailure);

            // Assert
            Assert.AreEqual("Not Found", result);
        }
    }
}
