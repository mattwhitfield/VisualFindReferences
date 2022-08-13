namespace VisualFindReferences.Core.Tests.Graph.Layout
{
    using VisualFindReferences.Core.Graph.Layout;
    using System;
    using Xunit;
    using FluentAssertions;

    public class FRLayoutParametersTests
    {
        private FRLayoutParameters _testClass;

        public FRLayoutParametersTests()
        {
            _testClass = new FRLayoutParameters();
            _testClass.VertexCount = 20;
        }

        [Fact]
        public void CanSetAndGetVertexCount()
        {
            // Arrange
            var testValue = 1030435372;

            // Act
            _testClass.VertexCount = testValue;

            // Assert
            _testClass.VertexCount.Should().Be(testValue);
        }

        [Fact]
        public void CanGetK()
        {
            // Assert
            _testClass.K.Should().Be(50);
        }

        [Fact]
        public void CanGetInitialTemperature()
        {
            // Assert
            _testClass.InitialTemperature.Should().Be(Math.Sqrt(Math.Pow(_testClass.IdealEdgeLength, 2) * _testClass.VertexCount));
        }

        [Fact]
        public void CanSetAndGetIdealEdgeLength()
        {
            // Arrange
            var testValue = 1575708984.6299999;

            // Act
            _testClass.IdealEdgeLength = testValue;

            // Assert
            _testClass.IdealEdgeLength.Should().Be(testValue);
        }

        [Fact]
        public void CanGetConstantOfAttraction()
        {
            // Assert
            _testClass.ConstantOfAttraction.Should().Be(_testClass.K * _testClass.AttractionMultiplier);
        }

        [Fact]
        public void CanSetAndGetAttractionMultiplier()
        {
            // Arrange
            var testValue = 259595270.55;

            // Act
            _testClass.AttractionMultiplier = testValue;

            // Assert
            _testClass.AttractionMultiplier.Should().Be(testValue);
        }

        [Fact]
        public void CanGetConstantOfRepulsion()
        {
            // Assert
            _testClass.ConstantOfRepulsion.Should().Be(Math.Pow(50 * 0.6, 2));
        }

        [Fact]
        public void CanSetAndGetRepulsiveMultiplier()
        {
            // Arrange
            var testValue = 38104236.719999991;

            // Act
            _testClass.RepulsiveMultiplier = testValue;

            // Assert
            _testClass.RepulsiveMultiplier.Should().Be(testValue);
        }

        [Fact]
        public void CanSetAndGetMaxIterations()
        {
            // Arrange
            var testValue = 1877183760;

            // Act
            _testClass.MaxIterations = testValue;

            // Assert
            _testClass.MaxIterations.Should().Be(testValue);
        }

        [Fact]
        public void CanSetAndGetLambda()
        {
            // Arrange
            var testValue = 1164143775.96;

            // Act
            _testClass.Lambda = testValue;

            // Assert
            _testClass.Lambda.Should().Be(testValue);
        }
    }
}