using System.Threading.Tasks;
using Xunit;

// ReSharper disable once CheckNamespace - avoid name collision with subject under test
namespace YakShaveFx.FunctionalExtensions.Tests
{
    public class MaybeExtensionsMatchTests
    {
        [Fact]
        public void When_Matching_Some_Or_None_With_Some_Then_Some_Action_Is_Invoked_With_Value()
        {
            const string originalValue = nameof(originalValue);
            var maybeValue = Maybe.Some(originalValue);
            var matchedValue = "Will be replaced";

            maybeValue.Match(value => matchedValue = value, () => matchedValue = "Won't be called");

            Assert.Equal(originalValue, matchedValue);
        }

        [Fact]
        public void When_Matching_Some_Or_None_With_None_Then_None_Action_Is_Invoked()
        {
            var maybeValue = Maybe.None<string>();
            var matchedValue = "Will be replaced";

            maybeValue.Match(value => matchedValue = "Won't be called", () => matchedValue = "None matched");

            Assert.Equal("None matched", matchedValue);
        }

        [Fact]
        public async Task When_Matching_Some_Or_None_Async_With_Some_Then_Some_Action_Is_Invoked_With_Value()
        {
            const string originalValue = nameof(originalValue);
            var maybeValue = Maybe.Some(originalValue);
            var matchedValue = "Will be replaced";

            await maybeValue.MatchAsync(value =>
            {
                matchedValue = value;
                return Task.CompletedTask;
            }, () =>
            {
                matchedValue = "Won't be called";
                return Task.CompletedTask;
            });

            Assert.Equal(originalValue, matchedValue);
        }

        [Fact]
        public async Task When_Matching_Some_Or_None_Async_With_None_Then_None_Action_Is_Invoked()
        {
            var maybeValue = Maybe.None<string>();
            var matchedValue = "Will be replaced";

            await maybeValue.MatchAsync(value =>
            {
                matchedValue = "Won't be called";
                return Task.CompletedTask;
            }, () =>
            {
                matchedValue = "None matched";
                return Task.CompletedTask;
            });

            Assert.Equal("None matched", matchedValue);
        }
        
        [Fact]
        public void When_Matching_Some_With_Some_Then_Action_Is_Invoked_With_Value()
        {
            const string originalValue = nameof(originalValue);
            var maybeValue = Maybe.Some(originalValue);
            var matchedValue = "Will be replaced";

            maybeValue.MatchSome(value => matchedValue = value);

            Assert.Equal(originalValue, matchedValue);
        }

        [Fact]
        public void When_Matching_Some_With_None_Then_No_Action_Is_Invoked()
        {
            var maybeValue = Maybe.None<string>();
            var matchedValue = "Won't be replaced";

            maybeValue.MatchSome(value => matchedValue = "Won't be called");

            Assert.Equal("Won't be replaced", matchedValue);
        }

        [Fact]
        public async Task When_Matching_Some_Async_With_Some_Then_Action_Is_Invoked_With_Value()
        {
            const string originalValue = nameof(originalValue);
            var maybeValue = Maybe.Some(originalValue);
            var matchedValue = "Will be replaced";

            await maybeValue.MatchSomeAsync(value =>
            {
                matchedValue = value;
                return Task.CompletedTask;
            });

            Assert.Equal(originalValue, matchedValue);
        }

        [Fact]
        public async Task When_Matching_Some_Async_With_None_Then_No_Action_Is_Invoked()
        {
            var maybeValue = Maybe.None<string>();
            var matchedValue = "Won't be replaced";

            await maybeValue.MatchSomeAsync(value =>
            {
                matchedValue = "Won't be called";
                return Task.CompletedTask;
            });

            Assert.Equal("Won't be replaced", matchedValue);
        }
        
        [Fact]
        public void When_Matching_None_With_Some_Then_No_Action_Is_Invoked()
        {
            const string originalValue = nameof(originalValue);
            var maybeValue = Maybe.Some(originalValue);
            var matchedValue = "Won't be replaced";

            maybeValue.MatchNone(() => matchedValue = "Won't be called");

            Assert.Equal("Won't be replaced", matchedValue);
        }

        [Fact]
        public void When_Matching_None_With_None_Then_Action_Is_Invoked()
        {
            var maybeValue = Maybe.None<string>();
            var matchedValue = "Will be replaced";

            maybeValue.MatchNone(() => matchedValue = "Matched none");

            Assert.Equal("Matched none", matchedValue);
        }

        [Fact]
        public async Task When_Matching_None_Async_With_Some_Then_No_Action_Is_Invoked()
        {
            const string originalValue = nameof(originalValue);
            var maybeValue = Maybe.Some(originalValue);
            var matchedValue = "Won't be replaced";

            await maybeValue.MatchNoneAsync(() =>
            {
                matchedValue = "Won't be called";
                return Task.CompletedTask;
            });

            Assert.Equal("Won't be replaced", matchedValue);
        }

        [Fact]
        public async Task When_Matching_None_Async_With_None_Then_Action_Is_Invoked()
        {
            var maybeValue = Maybe.None<string>();
            var matchedValue = "Will be replaced";

            await maybeValue.MatchNoneAsync(() =>
            {
                matchedValue = "Matched none";
                return Task.CompletedTask;
            });

            Assert.Equal("Matched none", matchedValue);
        }
    }
}