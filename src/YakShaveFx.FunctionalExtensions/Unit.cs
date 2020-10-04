using System;

namespace YakShaveFx.FunctionalExtensions
{
    /// <summary>
    /// Meant to be interpreted as void, but as a type, to allow generic types and operations to use it instead of creating multiple variations.
    /// </summary>
    /// <remarks>For more info, see https://en.wikipedia.org/wiki/Unit_type.</remarks>
    public struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
    {
        /// <summary>
        /// Default and only value of the <see cref="Unit"/> type.
        /// </summary>
        public static readonly Unit Value = new Unit();

        /// <inheritdoc />
        public bool Equals(Unit other) => true;

        /// <inheritdoc />
        public int CompareTo(Unit other) => 0;

        /// <inheritdoc />
        public int CompareTo(object? obj)
            => obj is Unit ? 0 : throw new ArgumentException("Unit should only be compared to other Units.");

        /// <inheritdoc />
        public override int GetHashCode() => 0;

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is Unit;

        /// <summary>
        /// Determines whether two specified <see cref="Unit"/> have the same values. 
        /// </summary>
        /// <param name="left">The first <see cref="Unit"/> to compare.</param>
        /// <param name="right">The second <see cref="Unit"/> to compare.</param>
        /// <returns><c>true</c> if the value of <paramref name="left"/> is the same as the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Unit left, Unit right) => true;

        /// <summary>
        /// Determines whether two specified <see cref="Unit"/> have different values.
        /// </summary>
        /// <param name="left">The first <see cref="Unit"/> to compare.</param>
        /// <param name="right">The second <see cref="Unit"/> to compare.</param>
        /// <returns><c>true</c> if the value of <paramref name="left"/> is different to the value of <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Unit left, Unit right) => false;
    }
}