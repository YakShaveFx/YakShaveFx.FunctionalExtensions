using Xunit;

// ReSharper disable once CheckNamespace - avoid name collision with subject under test
namespace YakShaveFx.FunctionalExtensions.Tests
{
    public class MaybeExtensionsMapTests
    {
        [Fact]
        public void When_Mapping_Some_A_New_Some_Maybe_Is_Returned()
        {
            var originalValue = new object();
            var maybeObject = Maybe.Some(originalValue);

            var maybeMapped = maybeObject.Map(value => "Mapped");
            
            Assert.True(maybeMapped.TryGetValue(out var mapped));
            Assert.Equal("Mapped", mapped);
        }
        
        [Fact]
        public void When_Mapping_None_A_New_None_Maybe_Is_Returned()
        {
            var maybeObject = Maybe.None<object>();

            var maybeMapped = maybeObject.Map(value => "Won't be used");
            
            Assert.False(maybeMapped.TryGetValue(out _));
        }
        
        [Fact]
        public void When_Mapping_ValueOr_With_Some_A_The_Mapped_Value_Is_Returned()
        {
            var originalValue = new object();
            var maybeObject = Maybe.Some(originalValue);

            var mapped = maybeObject.MapValueOr(value => "Mapped", () => "Won't be used");
            
            Assert.Equal("Mapped", mapped);
        }
        
        [Fact]
        public void When_Mapping_ValueOr_With_None_The_None_Or_Value_Is_Returned()
        {
            var maybeObject = Maybe.None<object>();

            var mapped = maybeObject.MapValueOr(value => "Won't be used", () => "None");
            
            Assert.Equal("None", mapped);
        }
    }
}