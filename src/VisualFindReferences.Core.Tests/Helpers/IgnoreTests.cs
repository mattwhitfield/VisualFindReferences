namespace VisualFindReferences.Core.Tests.Helpers
{
    using VisualFindReferences.Core.Helpers;
    using Xunit;
    using FluentAssertions;

    public static class IgnoreTests
    {
        [Fact]
        public static void CanCallHResult()
        {
            // Arrange
            var result = 2092678008;

            // Act
            FluentActions.Invoking(() => Ignore.HResult(result)).Should().NotThrow();
        }
    }
}