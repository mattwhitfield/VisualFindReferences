namespace VisualFindReferences.Core.Tests.Graph.View
{
    using VisualFindReferences.Core.Graph.View;
    using System;
    using Xunit;
    using FluentAssertions;
    using VisualFindReferences.Core.Graph.Layout;
    using System.Windows.Media;

    public class ZoomAndPanTests
    {
        private ZoomAndPan _testClass;

        public ZoomAndPanTests()
        {
            _testClass = new ZoomAndPan();
        }

        [Fact]
        public void CanCallGetTarget()
        {
            // Arrange
            var area = new GraphRect(10, 10, 80, 80);

            // Act
            var result = _testClass.GetTarget(area);

            // Assert
            result.StartX.Should().Be(2.5);
            result.StartY.Should().Be(2.5);
            result.Scale.Should().Be(0.05);
        }

        [Fact]
        public void CanCallConstrainScale()
        {
            // Arrange
            var scale = 1127912475.69;

            // Act
            var result = ZoomAndPan.ConstrainScale(scale);

            // Assert
            result.Should().Be(3);
        }

        [Fact]
        public void CanSetAndGetViewWidth()
        {
            // Arrange
            var testValue = 803486521.53;

            // Act
            _testClass.ViewWidth = testValue;

            // Assert
            _testClass.ViewWidth.Should().Be(testValue);
        }

        [Fact]
        public void CanSetAndGetViewHeight()
        {
            // Arrange
            var testValue = 739086550.29;

            // Act
            _testClass.ViewHeight = testValue;

            // Assert
            _testClass.ViewHeight.Should().Be(testValue);
        }

        [Fact]
        public void CanSetAndGetStartX()
        {
            // Arrange
            var testValue = 136134734.67;

            // Act
            _testClass.StartX = testValue;

            // Assert
            _testClass.StartX.Should().Be(testValue);
        }

        [Fact]
        public void CanSetAndGetStartY()
        {
            // Arrange
            var testValue = 1004900064.3;

            // Act
            _testClass.StartY = testValue;

            // Assert
            _testClass.StartY.Should().Be(testValue);
        }

        [Fact]
        public void CanSetAndGetScale()
        {
            // Arrange
            var testValue = 267076322.37;

            // Act
            _testClass.Scale = testValue;

            // Assert
            _testClass.Scale.Should().Be(testValue);
        }

        [Fact]
        public void CanSetAndGetMatrix()
        {
            // Arrange
            var testValue = new Matrix();

            // Act
            _testClass.Matrix = testValue;

            // Assert
            _testClass.Matrix.Should().Be(testValue);
        }

        [Fact]
        public void CanGetMatrixInv()
        {
            // Assert
            _testClass.MatrixInv.As<object>().Should().BeAssignableTo<Matrix>();
        }
    }
}