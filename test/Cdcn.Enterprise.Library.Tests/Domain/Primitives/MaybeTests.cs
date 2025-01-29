using Cdcn.Enterprise.Library.Domain.Primitives;
using Cdcn.Enterprise.Library.Domain.Primitives.Maybe;

namespace Cdcn.Enterprise.Library.Domain.Tests.Primitives.Maybe
{
    [TestFixture]
    public class MaybeTests
    {
        [Test]
        public void Maybe_HasValue_ShouldReturnTrue_WhenValueIsPresent()
        {
            var maybe = Maybe<int>.From(5);
            Assert.IsTrue(maybe.HasValue);
        }

        [Test]
        public void Maybe_Value_ShouldReturnValue_WhenValueIsPresent()
        {
            var maybe = Maybe<int>.From(5);
            Assert.AreEqual(5, maybe.Value);
        }

        [Test]
        public void Maybe_None_ShouldReturnMaybeWithNoValue()
        {
            var maybe = Maybe<int>.None;
            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual("None", maybe.Error.Code);
            Assert.AreEqual("Not Exist", maybe.Error.Message);
        }

        [Test]
        public void Maybe_Exception_ShouldReturnMaybeWithError()
        {
            var error = new Error("Error", "An error occurred");
            var maybe = Maybe<int>.Exception(error);
            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual("Error", maybe.Error.Code);
            Assert.AreEqual("An error occurred", maybe.Error.Message);
        }

        [Test]
        public void Maybe_From_ShouldReturnMaybeWithValue()
        {
            var maybe = Maybe<int>.From(5);
            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(5, maybe.Value);
        }

        [Test]
        public void Maybe_ImplicitConversionToMaybe_ShouldReturnMaybeWithValue()
        {
            Maybe<int> maybe = 5;
            Assert.IsTrue(maybe.HasValue);
            Assert.AreEqual(5, maybe.Value);
        }

        [Test]
        public void Maybe_ImplicitConversionToValue_ShouldReturnValue()
        {
            var maybe = Maybe<int>.From(5);
            int value = maybe;
            Assert.AreEqual(5, value);
        }

        [Test]
        public void Maybe_Equals_ShouldReturnTrue_WhenBothMaybesHaveSameValue()
        {
            var maybe1 = Maybe<int>.From(5);
            var maybe2 = Maybe<int>.From(5);
            Assert.IsTrue(maybe1.Equals(maybe2));
        }

        [Test]
        public void Maybe_Equals_ShouldReturnFalse_WhenMaybesHaveDifferentValues()
        {
            var maybe1 = Maybe<int>.From(5);
            var maybe2 = Maybe<int>.From(10);
            Assert.IsFalse(maybe1.Equals(maybe2));
        }

        [Test]
        public void Maybe_Equals_ShouldReturnFalse_WhenOtherIsNull()
        {
            var maybe = Maybe<int>.From(5);
            Assert.IsFalse(maybe.Equals(null));
        }

        [Test]
        public void Maybe_Equals_ShouldReturnTrue_WhenBothMaybesHaveNoValue()
        {
            var maybe1 = Maybe<int>.None;
            var maybe2 = Maybe<int>.None;
            Assert.IsTrue(maybe1.Equals(maybe2));
        }

        [Test]
        public void Maybe_GetHashCode_ShouldReturnHashCodeOfValue_WhenValueIsPresent()
        {
            var maybe = Maybe<int>.From(5);
            Assert.AreEqual(5.GetHashCode(), maybe.GetHashCode());
        }

        [Test]
        public void Maybe_GetHashCode_ShouldReturnZero_WhenValueIsAbsent()
        {
            var maybe = Maybe<int>.None;
            Assert.AreEqual(0, maybe.GetHashCode());
        }
    }
}