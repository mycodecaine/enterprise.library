using Cdcn.Enterprise.Library.Domain.Primitives;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Tests.Domain.Primitives
{
    [TestFixture]
    public class ValueObjectTests
    {
        [Test]
        public void EqualValueObjectsShouldBeEqual()
        {
            var valueObject1 = new SampleValueObject(1, "Test");
            var valueObject2 = new SampleValueObject(1, "Test");

            Assert.IsTrue(valueObject1 == valueObject2);
            Assert.IsFalse(valueObject1 != valueObject2);
            Assert.AreEqual(valueObject1, valueObject2);
        }

        [Test]
        public void DifferentValueObjectsShouldNotBeEqual()
        {
            var valueObject1 = new SampleValueObject(1, "Test");
            var valueObject2 = new SampleValueObject(2, "Test");

            Assert.IsFalse(valueObject1 == valueObject2);
            Assert.IsTrue(valueObject1 != valueObject2);
            Assert.AreNotEqual(valueObject1, valueObject2);
        }

        [Test]
        public void NullValueObjectShouldNotEqualNonNull()
        {
            var valueObject = new SampleValueObject(1, "Test");
            SampleValueObject? nullValueObject = null;
           
            Assert.IsFalse(valueObject == nullValueObject);
            Assert.IsNull(nullValueObject);
        }

        [Test]
        public void SameInstanceShouldBeEqual()
        {
            var valueObject = new SampleValueObject(1, "Test");

            Assert.AreEqual(valueObject, valueObject);
        }

        [Test]
        public void GetHashCodeShouldBeConsistent()
        {
            var valueObject1 = new SampleValueObject(1, "Test");
            var valueObject2 = new SampleValueObject(1, "Test");

            Assert.AreEqual(valueObject1.GetHashCode(), valueObject2.GetHashCode());
        }

        [Test]
        public void DifferentValuesShouldHaveDifferentHashCodes()
        {
            var valueObject1 = new SampleValueObject(1, "Test");
            var valueObject2 = new SampleValueObject(2, "Test");

            Assert.AreNotEqual(valueObject1.GetHashCode(), valueObject2.GetHashCode());
        }     

        [Test]
        public void Equals_ShouldReturnFalse_WhenComparedToNull()
        {
            var valueObject = new SampleValueObject(1, "Test");
            Assert.IsFalse(valueObject.Equals(null));
        }

        [Test]
        public void Equals_ShouldReturnFalse_WhenComparedToDifferentType()
        {
            var valueObject = new SampleValueObject(1, "Test");
            var differentTypeObject = new object();

            Assert.IsFalse(valueObject.Equals(differentTypeObject));
        }

        [Test]
        public void Equals_ShouldReturnFalse_WhenComparedToNonValueObject()
        {
            var valueObject = new SampleValueObject(1, "Test");
            var nonValueObject = new List<string>();

            Assert.IsFalse(valueObject.Equals(nonValueObject));
        }
    }

    public class SampleValueObject : ValueObject
    {
        public int Id { get; }
        public string Name { get; }

        public SampleValueObject(int id, string name)
        {
            Id = id;
            Name = name;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Name;
        }
    }


}
