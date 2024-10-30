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
        private readonly T? _value; // Allow null-able here

        private Maybe(T? value) => _value = value;

        private Maybe(T? value, Error error)
        {
            _value = value;
            Error = error;
        }

        public bool HasValue => !HasNoValue;

        public bool HasNoValue => _value is null;

        public T Value => HasValue
            ? _value!
            : throw new InvalidOperationException("The value cannot be accessed because it does not exist.");

        public static Maybe<T> None => new Maybe<T>(default(T), new Error("None", "Not Exist"));
        public static Maybe<T> Exception(Error error) => new Maybe<T>(default(T), error);

        public static Maybe<T> From(T value) => new Maybe<T>(value);

        public static implicit operator Maybe<T>(T value) => From(value);

        public static implicit operator T(Maybe<T> maybe) => maybe.Value;

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

        public override int GetHashCode() => HasValue && Value is not null ? Value.GetHashCode() : 0;

        public Error Error { get; }
    }

}
