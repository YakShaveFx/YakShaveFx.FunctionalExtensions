using System;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace - folder used for project organization, not useful for library users
namespace YakShaveFx.FunctionalExtensions
{
    public static class MaybeExtensions
    {
        public static Maybe<TOut> Map<TIn, TOut>(this Maybe<TIn> maybeValue, Func<TIn, TOut> mapper)
            where TIn : notnull
            where TOut : notnull
        {
            if (mapper is null) throw new ArgumentNullException(nameof(mapper));

            return maybeValue.TryGetValue(out var value)
                ? Maybe.Some(mapper(value))
                : Maybe.None<TOut>();
        }

        public static T ValueOr<T>(this Maybe<T> maybeValue, Func<T> alternativeFactory)
            where T : notnull
        {
            if (alternativeFactory is null) throw new ArgumentNullException(nameof(alternativeFactory));

            return maybeValue.TryGetValue(out var value)
                ? value
                : alternativeFactory();
        }

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
        
        public static Task MatchAsync<T>(this Maybe<T> maybeValue, Func<T, Task> some, Func<Task> none)
            where T : notnull
        {
            if (some is null) throw new ArgumentNullException(nameof(some));
            if (none is null) throw new ArgumentNullException(nameof(none));

            return maybeValue.TryGetValue(out var value)
                ? some(value)
                : none();
        }

        public static void MatchSome<T>(this Maybe<T> maybeValue, Action<T> some)
            where T : notnull
        {
            if (some is null) throw new ArgumentNullException(nameof(some));

            if (maybeValue.TryGetValue(out var value))
            {
                some(value);
            }
        }

        public static Task MatchSomeAsync<T>(this Maybe<T> maybeValue, Func<T, Task> some)
            where T : notnull
        {
            if (some is null) throw new ArgumentNullException(nameof(some));

            return maybeValue.TryGetValue(out var value) ? some(value) : Task.CompletedTask;
        }

        public static void MatchNone<T>(this Maybe<T> maybeValue, Action none)
            where T : notnull
        {
            if (none is null) throw new ArgumentNullException(nameof(none));

            if (!maybeValue.TryGetValue(out _))
            {
                none();
            }
        }

        public static Task MatchNoneAsync<T>(this Maybe<T> maybeValue, Func<Task> none)
            where T : notnull
        {
            if (none is null) throw new ArgumentNullException(nameof(none));

            return !maybeValue.TryGetValue(out _) ? none() : Task.CompletedTask;
        }
    }
}