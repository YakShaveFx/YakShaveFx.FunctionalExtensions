using System;
using Xunit;

// ReSharper disable once CheckNamespace - avoid name collision with subject under test
namespace YakShaveFx.FunctionalExtensions.Tests
{
    public class MaybeCreationTests
    {
        [Fact]
        public void When_Creating_Some_With_Value_It_Can_Be_Retrieved()
        {
            var originalValue = new object();
            var maybeObject = Maybe.Some(originalValue);
            Assert.True(maybeObject.TryGetValue(out var value));
            Assert.Equal(originalValue, value);
        }

        [Fact]
        public void When_Creating_Some_With_Null_Value_An_Exception_Is_Thrown()
        {
            // just to sanity check the exception
#pragma warning disable 8714
            // ReSharper disable once HeapView.BoxingAllocation
            Assert.Throws<ArgumentNullException>(() => Maybe.Some((object?) null));
#pragma warning restore 8714
        }

        [Fact]
        public void When_Creating_None_No_Value_CanBe_Retrieved()
        {
            var maybeObject = Maybe.None<object>();
            Assert.False(maybeObject.TryGetValue(out _));
        }

        [Fact]
        public void When_Creating_From_Not_Null_Nullable_Reference_Type_The_Value_Can_Be_Retrieved()
        {
            var originalValue = new object();
            var maybeObject = Maybe.FromNullable(originalValue);
            Assert.True(maybeObject.TryGetValue(out var value));
            Assert.Equal(originalValue, value);
        }

        [Fact]
        public void When_Creating_From_Not_Null_Nullable_Value_Type_The_Value_Can_Be_Retrieved()
        {
            int? originalValue = 1;
            var maybeObject = Maybe.FromNullable(originalValue);
            Assert.True(maybeObject.TryGetValue(out var value));
            Assert.Equal(originalValue, value);
        }

        [Fact]
        public void When_Creating_From_Null_Nullable_Reference_Type_The_Value_Can_Be_Retrieved()
        {
            var maybeObject = Maybe.FromNullable((object?) null);
            Assert.False(maybeObject.TryGetValue(out _));
        }

        [Fact]
        public void When_Creating_From_Null_Nullable_Value_Type_The_Value_Can_Be_Retrieved()
        {
            var maybeObject = Maybe.FromNullable((int?) null);
            Assert.False(maybeObject.TryGetValue(out _));
        }
    }
}