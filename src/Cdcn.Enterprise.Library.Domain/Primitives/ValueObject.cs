namespace Cdcn.Enterprise.Library.Domain.Primitives
{
    /// <summary>
    /// Represents a base class for value objects.
    /// </summary>
    public abstract class ValueObject : IEquatable<ValueObject>
    {
        /// <summary>
        /// Determines whether two value object instances are equal.
        /// </summary>
        /// <param name="a">The first value object.</param>
        /// <param name="b">The second value object.</param>
        /// <returns>True if the value objects are equal; otherwise, false.</returns>
        public static bool operator ==(ValueObject a, ValueObject b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether two value object instances are not equal.
        /// </summary>
        /// <param name="a">The first value object.</param>
        /// <param name="b">The second value object.</param>
        /// <returns>True if the value objects are not equal; otherwise, false.</returns>
        public static bool operator !=(ValueObject a, ValueObject b) => !(a == b);

        /// <inheritdoc />
        public bool Equals(ValueObject? other) => !(other is null) && GetAtomicValues().SequenceEqual(other.GetAtomicValues());

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            if (!(obj is ValueObject valueObject))
            {
                return false;
            }

            return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            HashCode hashCode = default;

            foreach (object obj in GetAtomicValues())
            {
                hashCode.Add(obj);
            }

            return hashCode.ToHashCode();
        }

        /// <summary>
        /// Gets the atomic values of the value object.
        /// </summary>
        /// <returns>The collection of objects representing the value object values.</returns>
        protected abstract IEnumerable<object> GetAtomicValues();
    }
}
