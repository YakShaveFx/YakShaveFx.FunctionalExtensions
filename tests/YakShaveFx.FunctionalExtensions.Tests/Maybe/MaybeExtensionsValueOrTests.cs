using Xunit;

// ReSharper disable once CheckNamespace - avoid name collision with subject under test
namespace YakShaveFx.FunctionalExtensions.Tests
{
    public class MaybeExtensionsValueOrTests
    {
        [Fact]
        public void When_Getting_ValueOr_With_Some_The_Value_Is_Returned()
        {
            const string originalValue = nameof(originalValue);
            var maybeValue = Maybe.Some(originalValue);

            var value = maybeValue.ValueOr(() => "Won't be used");

            Assert.Equal(originalValue, value);
        }

        [Fact]
        public void When_Getting_ValueOr_With_None_The_Factory_Value_Is_Returned()
        {
            var maybeValue = Maybe.None<string>();

            var value = maybeValue.ValueOr(() => "None");

            Assert.Equal("None", value);
        }
    }
}