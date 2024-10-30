using Cdcn.Enterprise.Library.Domain.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cdcn.Enterprise.Library.Domain.Primitives.Maybe
{
    public sealed class Maybe<T> : IEquatable<Maybe<T>>
    {
        private readonly T _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Maybe{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        private Maybe(T value) => _value = value;

        private Maybe(T value, Error error)
        {
            _value = value;
            Error = error;
        }

        /// <summary>
        /// Gets a value indicating whether or not the value exists.
        /// </summary>
        public bool HasValue => !HasNoValue;

        /// <summary>
        /// Gets a value indicating whether or not the value does not exist.
        /// </summary>
        public bool HasNoValue => _value is null;

        /// <summary>
        /// Gets the value.
        /// </summary>
        public T Value => HasValue
            ? _value
            : throw new InvalidOperationException("The value can not be accessed because it does not exist.");

        /// <summary>
        /// Gets the default empty instance.
        /// </summary>
        public static Maybe<T> None => new Maybe<T>(default, new Error("None", "Not Exist"));
        public static Maybe<T> Exception(Error error) => new Maybe<T>(default, error);


        /// <summary>
        /// Creates a new <see cref="Maybe{T}"/> instance based on the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The new <see cref="Maybe{T}"/> instance.</returns>
        public static Maybe<T> From(T value) => new Maybe<T>(value);

        public static implicit operator Maybe<T>(T value) => From(value);

        public static implicit operator T(Maybe<T> maybe) => maybe.Value;

        /// <inheritdoc />
        public bool Equals(Maybe<T> other)
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

            return Value.Equals(other.Value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) =>
            obj switch
            {
                null => false,
                T value => Equals(new Maybe<T>(value)),
                Maybe<T> maybe => Equals(maybe),
                _ => false
            };

        /// <inheritdoc />
        public override int GetHashCode() => HasValue ? Value.GetHashCode() : 0;
        public Error Error { get; }
    }
}
