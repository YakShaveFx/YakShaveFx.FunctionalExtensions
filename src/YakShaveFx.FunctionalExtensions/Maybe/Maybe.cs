using System;
using System.Collections.Generic;

#if NETSTANDARD2_1
using System.Diagnostics.CodeAnalysis;
#endif

// ReSharper disable once CheckNamespace - folder used for project organization, not useful for library users
namespace YakShaveFx.FunctionalExtensions
{
    public static class Maybe
    {
        public static Maybe<T> Some<T>(T value) where T : notnull => new Maybe<T>(value);

        public static Maybe<T> None<T>() where T : notnull => new Maybe<T>();

        public static Maybe<T> FromNullable<T>(T? value) where T : class
            => value is null
                ? None<T>()
                : Some(value);

        public static Maybe<T> FromNullable<T>(T? value) where T : struct
            => value.HasValue
                ? Some(value.Value)
                : None<T>();
    }

    public readonly struct Maybe<T> : IEquatable<Maybe<T>>, IComparable<Maybe<T>>, IComparable
        where T : notnull
    {
        private readonly T _value;

        public bool HasValue { get; }

        internal Maybe(T value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
            HasValue = true;
        }

#if NETSTANDARD2_1
        public bool TryGetValue([MaybeNullWhen(false)]out T value)
        {
            value = HasValue ? _value : default;
            return HasValue;
        }
#else
        public bool TryGetValue(out T value)
        {
#pragma warning disable 8601
            // no other choice but assign default here, even if it's null
            value = HasValue ? _value : default;
#pragma warning restore 8601
            return HasValue;
        }
#endif

        /// <inheritdoc />
        public bool Equals(Maybe<T> other)
        {
            if (!HasValue && !other.HasValue)
                return true;

            if (HasValue != other.HasValue)
                return false;

            return _value.Equals(other._value);
        }

        /// <inheritdoc />
        public int CompareTo(Maybe<T> other)
        {
            return (HasValue, other.HasValue) switch
            {
                (false, false) => 0,
                (false, true) => -1,
                (true, false) => 1,
                (true, true) => CompareValues(_value, other._value)
            };

            static int CompareValues(T value, T otherValue)
                => value is IComparable<T> comparableValue
                    ? comparableValue.CompareTo(otherValue)
                    : Comparer<T>.Default.Compare(value, otherValue);
        }


        /// <inheritdoc />
        public int CompareTo(object? obj)
            => obj switch
            {
                Maybe<T> other => CompareTo(other),
                _ => throw new ArgumentException(
                    "Maybe should only be compared to other Maybe with same generic argument.")
            };

        /// <inheritdoc />
        public override int GetHashCode()
            => HasValue
                ? HasValue.GetHashCode()
                : HasValue.GetHashCode() * 17 + _value.GetHashCode();

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is Maybe<T> other && Equals(other);

        /// <summary>
        /// Determines whether two specified <see cref="Unit"/> have the same values. 
        /// </summary>
        /// <param name="left">The first <see cref="Unit"/> to compare.</param>
        /// <param name="right">The second <see cref="Unit"/> to compare.</param>
        /// <returns><c>true</c> if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Maybe<T> left, Maybe<T> right) => left.Equals(right);

        /// <summary>
        /// Determines whether two specified <see cref="Unit"/> have different values.
        /// </summary>
        /// <param name="left">The first <see cref="Unit"/> to compare.</param>
        /// <param name="right">The second <see cref="Unit"/> to compare.</param>
        /// <returns><c>true</c> if the value of <paramref name="left"/> is different to the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Maybe<T> left, Maybe<T> right) => !left.Equals(right);
    }
}