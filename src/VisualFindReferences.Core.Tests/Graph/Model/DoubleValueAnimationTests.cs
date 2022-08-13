namespace VisualFindReferences.Core.Tests.Graph.Model
{
    using VisualFindReferences.Core.Graph.Model;
    using Xunit;
    using FluentAssertions;

    public class DoubleValueAnimationTests
    {
        private DoubleValueAnimation _testClass;
        private double _from;
        private double _to;

        public DoubleValueAnimationTests()
        {
            _from = 1;
            _to = 11;
            _testClass = new DoubleValueAnimation(_from, _to);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new DoubleValueAnimation(_from, _to);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanCallLerp()
        {
            // Arrange
            var factor = 0.5;

            // Act
            var result = _testClass.Lerp(factor);

            // Assert
            result.Should().Be(6);
        }
    }
}