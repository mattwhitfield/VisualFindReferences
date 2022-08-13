namespace VisualFindReferences.Core.Tests.Graph.Helper
{
    using VisualFindReferences.Core.Graph.Helper;
    using T = System.String;
    using System;
    using Xunit;
    using FluentAssertions;
    using System.Collections.Generic;

    public static class EnumerableExtensionsTests
    {
        [Fact]
        public static void CanCallEach()
        {
            // Arrange
            var list = new List<string>();
            var enumerable = new[] { "TestValue500532074", "TestValue1632281347", "TestValue2025753526" };
            Action<T> action = x => list.Add(x);

            // Act
            enumerable.Each<T>(action);

            // Assert
            list.Should().Equal(enumerable);
        }
    }
}