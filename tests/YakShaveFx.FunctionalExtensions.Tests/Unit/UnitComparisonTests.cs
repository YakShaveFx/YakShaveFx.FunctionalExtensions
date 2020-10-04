using System;
using System.Linq;
using Xunit;

// ReSharper disable once CheckNamespace - avoid name collision with subject under test
namespace YakShaveFx.FunctionalExtensions.Tests
{
    public class UnitComparisonTests
    {
        [Fact]
        public void When_Creating_A_New_Unit_It_Should_Be_Equal_To_The_Singleton()
        {
            var created = new Unit();
            var singleton = Unit.Value;
            Assert.Equal(singleton, created);
        }

        [Fact]
        public void When_Getting_Default_Unit_Value_It_Should_Be_Equal_To_The_Singleton()
        {
            var @default = default(Unit);
            var singleton = Unit.Value;
            Assert.Equal(singleton, @default);
        }

        public static object[][] NotEqualToUnitData => new[]
        {
            new object[] {null!},
            new object[] {new object()},
            new object[] {0},
            new object[] {int.MaxValue},
            new object[] {string.Empty},
            new object[] {"Some value"}
        };

        [Theory]
        [MemberData(nameof(NotEqualToUnitData))]
        public void Should_Not_Be_Equal(object otherValue)
        {
            Assert.False(Unit.Value.Equals(otherValue));
        }
    }
}