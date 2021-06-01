using System;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace - folder used for project organization, not useful for library users
namespace YakShaveFx.FunctionalExtensions
{
    /// <summary>
    /// Provides common features to work with <see cref="Maybe{T}"/>.
    /// </summary>
    public static class MaybeExtensions
    {
        /// <summary>
        /// Maps a <typeparamref name="TIn"/> value to <typeparamref name="TOut"/>, if it is present.
        /// </summary>
        /// <param name="maybeValue">The <see cref="Maybe{T}"/> instance.</param>
        /// <param name="mapper">The function to map the value from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>.</param>
        /// <typeparam name="TIn">The type of the value wrapped by the input <see cref="Maybe{T}"/> instance.</typeparam>
        /// <typeparam name="TOut">The type of the value wrapped by the output <see cref="Maybe{T}"/> instance.</typeparam>
        /// <returns>A new instance of <see cref="Maybe{T}"/> with the mapped value (if present).</returns>
        public static Maybe<TOut> Map<TIn, TOut>(this Maybe<TIn> maybeValue, Func<TIn, TOut> mapper)
            where TIn : notnull
            where TOut : notnull
        {
            if (mapper is null) throw new ArgumentNullException(nameof(mapper));

            return maybeValue.TryGetValue(out var value)
                ? Maybe.Some(mapper(value))
                : Maybe.None<TOut>();
        }

        /// <summary>
        /// Returns the value if present, otherwise returns the result of the call to <paramref name="none"/>.
        /// </summary>
        /// <param name="maybeValue">The <see cref="Maybe{T}"/> instance.</param>
        /// <param name="none">The function to provide an alternative value if none is present.</param>
        /// <typeparam name="T">The type of the value wrapped by the <see cref="Maybe{T}"/> instance.</typeparam>
        /// <returns>The value if present, the alternative otherwise.</returns>
        public static T ValueOr<T>(this Maybe<T> maybeValue, Func<T> none)
            where T : notnull
        {
            if (none is null) throw new ArgumentNullException(nameof(none));

            return maybeValue.TryGetValue(out var value)
                ? value
                : none();
        }

        /// <summary>
        /// Returns the mapped value if present, otherwise returns the result of the call to <paramref name="none"/>.
        /// </summary>
        /// <param name="maybeValue">The <see cref="Maybe{T}"/> instance.</param>
        /// <param name="some">The function to map <typeparamref name="TIn"/> to <typeparamref name="TOut"/></param>
        /// <param name="none">The function to provide an alternative value if none is present.</param>
        /// <typeparam name="TIn">The type of the value wrapped by the input <see cref="Maybe{T}"/> instance.</typeparam>
        /// <typeparam name="TOut">The type of the value returned.</typeparam>
        /// <returns>The mapped value if present, an alternative otherwise.</returns>
        public static TOut MapValueOr<TIn, TOut>(this Maybe<TIn> maybeValue, Func<TIn, TOut> some, Func<TOut> none)
            where TIn : notnull
            where TOut : notnull
        {
            if (some is null) throw new ArgumentNullException(nameof(some));
            if (none is null) throw new ArgumentNullException(nameof(none));

            return maybeValue.TryGetValue(out var value)
                ? some(value)
                : none();
        }

        /// <summary>
        /// Invokes a specific function depending on the presence of value.
        /// </summary>
        /// <param name="maybeValue">The <see cref="Maybe{T}"/> instance.</param>
        /// <param name="some">The function to invoke when a value is present.</param>
        /// <param name="none">The function to invoke when a value is not present.</param>
        /// <typeparam name="T">The type of the value wrapped by the <see cref="Maybe{T}"/> instance.</typeparam>
        public static void Match<T>(this Maybe<T> maybeValue, Action<T> some, Action none)
            where T : notnull
        {
            if (some is null) throw new ArgumentNullException(nameof(some));
            if (none is null) throw new ArgumentNullException(nameof(none));

            if (maybeValue.TryGetValue(out var value))
                some(value);
            else
                none();
        }
        
        /// <summary>
        /// Invokes a specific function depending on the presence of value.
        /// </summary>
        /// <param name="maybeValue">The <see cref="Maybe{T}"/> instance.</param>
        /// <param name="some">The function to invoke when a value is present.</param>
        /// <param name="none">The function to invoke when a value is not present.</param>
        /// <typeparam name="T">The type of the value wrapped by the <see cref="Maybe{T}"/> instance.</typeparam>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static Task MatchAsync<T>(this Maybe<T> maybeValue, Func<T, Task> some, Func<Task> none)
            where T : notnull
        {
            if (some is null) throw new ArgumentNullException(nameof(some));
            if (none is null) throw new ArgumentNullException(nameof(none));

            return maybeValue.TryGetValue(out var value)
                ? some(value)
                : none();
        }

        /// <summary>
        /// Invokes a function if the value is present.
        /// </summary>
        /// <param name="maybeValue">The <see cref="Maybe{T}"/> instance.</param>
        /// <param name="some">The function to invoke when a value is present.</param>
        /// <typeparam name="T">The type of the value wrapped by the <see cref="Maybe{T}"/> instance.</typeparam>
        public static void MatchSome<T>(this Maybe<T> maybeValue, Action<T> some)
            where T : notnull
        {
            if (some is null) throw new ArgumentNullException(nameof(some));

            if (maybeValue.TryGetValue(out var value))
            {
                some(value);
            }
        }

        /// <summary>
        /// Invokes a function if the value is present.
        /// </summary>
        /// <param name="maybeValue">The <see cref="Maybe{T}"/> instance.</param>
        /// <param name="some">The function to invoke when a value is present.</param>
        /// <typeparam name="T">The type of the value wrapped by the <see cref="Maybe{T}"/> instance.</typeparam>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static Task MatchSomeAsync<T>(this Maybe<T> maybeValue, Func<T, Task> some)
            where T : notnull
        {
            if (some is null) throw new ArgumentNullException(nameof(some));

            return maybeValue.TryGetValue(out var value) ? some(value) : Task.CompletedTask;
        }

        /// <summary>
        /// Invokes a function if the value is not present.
        /// </summary>
        /// <param name="maybeValue">The <see cref="Maybe{T}"/> instance.</param>
        /// <param name="none">The function to invoke when a value is not present.</param>
        /// <typeparam name="T">The type of the value wrapped by the <see cref="Maybe{T}"/> instance.</typeparam>
        public static void MatchNone<T>(this Maybe<T> maybeValue, Action none)
            where T : notnull
        {
            if (none is null) throw new ArgumentNullException(nameof(none));

            if (!maybeValue.TryGetValue(out _))
            {
                none();
            }
        }

        /// <summary>
        /// Invokes a function if the value is not present.
        /// </summary>
        /// <param name="maybeValue">The <see cref="Maybe{T}"/> instance.</param>
        /// <param name="none">The function to invoke when a value is not present.</param>
        /// <typeparam name="T">The type of the value wrapped by the <see cref="Maybe{T}"/> instance.</typeparam>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static Task MatchNoneAsync<T>(this Maybe<T> maybeValue, Func<Task> none)
            where T : notnull
        {
            if (none is null) throw new ArgumentNullException(nameof(none));

            return !maybeValue.TryGetValue(out _) ? none() : Task.CompletedTask;
        }
    }
}