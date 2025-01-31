using Cdcn.Enterprise.Library.Domain.Primitives.Errors;

namespace Cdcn.Enterprise.Library.Domain.Primitives.Maybe
{
    /// <summary>
    /// Represents an optional value that may or may not exist, 
    /// providing a functional approach to handle the presence or absence of a value.
    /// Notes : <a href="https://chatgpt.com/share/67997334-0df0-8007-a768-123d7db54171"></a>
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public sealed class Maybe<T> : IEquatable<Maybe<T>>
    {
        private readonly T? _value; // Allow null-able here

        private Maybe(T? value) => _value = value;

        private Maybe(T? value, Error error)
        {
            _value = value;
            Error = error;
        }

        /// <summary>
        /// Indicates whether this instance contains a value.
        /// </summary>
        public bool HasValue => !HasNoValue;

        /// <summary>
        /// Indicates whether this instance does not contain a value.
        /// </summary>
        public bool HasNoValue => _value is null;

        public T Value => HasValue
            ? _value!
            : throw new InvalidOperationException("The value cannot be accessed because it does not exist.");

        /// <summary>
        /// Represents a "None" instance with no value and an associated error indicating absence.
        /// </summary>
        public static Maybe<T> None => new Maybe<T>(default(T), new Error("None", "Not Exist"));

        /// <summary>
        /// Creates a "Maybe" instance representing an error condition.
        /// </summary>
        /// <param name="error">The error associated with the instance.</param>
        /// <returns>A <see cref="Maybe{T}"/> representing an error.</returns>
        public static Maybe<T> Exception(Error error) => new Maybe<T>(default(T), error);

        /// <summary>
        /// Creates a "Maybe" instance from a given value.
        /// </summary>
        /// <param name="value">The value to wrap in a <see cref="Maybe{T}"/>.</param>
        /// <returns>A <see cref="Maybe{T}"/> containing the value.</returns>
        public static Maybe<T> From(T value) => new Maybe<T>(value);

        /// <summary>
        /// Implicitly converts a value of type <typeparamref name="T"/> to a <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        public static implicit operator Maybe<T>(T value) => From(value);

        /// <summary>
        /// Implicitly converts a <see cref="Maybe{T}"/> instance to its underlying value.
        /// </summary>
        /// <param name="maybe">The <see cref="Maybe{T}"/> instance to convert.</param>
        /// <exception cref="InvalidOperationException">Thrown if the instance has no value.</exception>
        public static implicit operator T(Maybe<T> maybe) => maybe.Value;

        /// <summary>
        /// Compares this instance with another <see cref="Maybe{T}"/> for equality.
        /// </summary>
        /// <param name="other">The other <see cref="Maybe{T}"/> to compare.</param>
        /// <returns>True if both instances are equal; otherwise, false.</returns>
        public bool Equals(Maybe<T>? other)
        {
            if (other is null)
            {
                return false;
            }

            if (HasNoValue && other.HasNoValue)
            {
                return true;
            }

            if (HasNoValue || other.HasNoValue)
            {
                return false;
            }

            // Safely compare values by ensuring `other.Value` is not null
            return Value is not null && Value.Equals(other.Value);
        }

        public override bool Equals(object? obj) =>
            obj switch
            {
                null => false,
                T value => Equals(new Maybe<T>(value)),
                Maybe<T> maybe => Equals(maybe),
                _ => false
            };

        /// <summary>
        /// Get Hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => HasValue && Value is not null ? Value.GetHashCode() : 0;

        /// <summary>
        /// Gets an <see cref="Error"/> associated with the current instance, if applicable.
        /// </summary>
        public Error Error { get; }
    }

}
