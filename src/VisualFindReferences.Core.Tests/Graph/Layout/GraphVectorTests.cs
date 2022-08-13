namespace VisualFindReferences.Core.Tests.Graph.Layout
{
    using VisualFindReferences.Core.Graph.Layout;
    using Xunit;
    using FluentAssertions;

    public class GraphVectorTests
    {
        private GraphVector _testClass;
        private double _x;
        private double _y;

        public GraphVectorTests()
        {
            _x = 1609720391.07;
            _y = 568353506.49;
            _testClass = new GraphVector(_x, _y);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new GraphVector(_x, _y);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void ImplementsIEquatable_GraphVector()
        {
            // Arrange
            var same = new GraphVector(_x, _y);
            var different = new GraphVector();

            // Assert
            _testClass.Equals(default(object)).Should().BeFalse();
            _testClass.Equals(new object()).Should().BeFalse();
            _testClass.Equals((object)same).Should().BeTrue();
            _testClass.Equals((object)different).Should().BeFalse();
            _testClass.Equals(same).Should().BeTrue();
            _testClass.Equals(different).Should().BeFalse();
            _testClass.GetHashCode().Should().Be(same.GetHashCode());
            _testClass.GetHashCode().Should().NotBe(different.GetHashCode());
            (_testClass == same).Should().BeTrue();
            (_testClass == different).Should().BeFalse();
            (_testClass != same).Should().BeFalse();
            (_testClass != different).Should().BeTrue();
        }

        [Fact]
        public void CanCallEqualsWithVector1AndVector2()
        {
            GraphVector.Equals(new GraphVector(10, 4), new GraphVector(10, 4)).Should().BeTrue();
            GraphVector.Equals(new GraphVector(10, 4), new GraphVector(10, 5)).Should().BeFalse();
        }

        [Fact]
        public void CanCallEqualsWithObj()
        {
            _testClass.Equals(new object()).Should().BeFalse();
            _testClass.Equals((object)new GraphVector(10, 4)).Should().BeFalse();
            _testClass.Equals((object)new GraphVector(_x, _y)).Should().BeTrue();
        }

        [Fact]
        public void CanCallEqualsWithOther()
        {
            // Arrange
            var other = new GraphVector(_x, _y);

            // Act
            var result = _testClass.Equals(other);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CanCallGetHashCode()
        {
            // Act
            var result = _testClass.GetHashCode();

            // Assert
            result.Should().Be(new GraphVector(_x, _y).GetHashCode());
        }

        [Fact]
        public void CanCallEqualityOperator()
        {
            // Arrange
            var vector1 = new GraphVector(10, 4);
            var vector2 = new GraphVector(10, 4);

            // Act
            var result = vector1 == vector2;

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CanCallInequalityOperator()
        {
            // Arrange
            var vector1 = new GraphVector(10, 4);
            var vector2 = new GraphVector(10, 4);

            // Act
            var result = vector1 != vector2;

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CanCallPlusOperator()
        {
            // Arrange
            var vector1 = new GraphVector(10, 20);
            var vector2 = new GraphVector(7, 9);

            // Act
            var result = vector1 + vector2;

            // Assert
            result.Should().BeEquivalentTo(new GraphVector(17, 29));
        }

        [Fact]
        public void CanCallMinusOperator()
        {
            // Arrange
            var vector1 = new GraphVector(10, 20);
            var vector2 = new GraphVector(7, 9);

            // Act
            var result = vector1 - vector2;

            // Assert
            result.Should().BeEquivalentTo(new GraphVector(3, 11));
        }

        [Fact]
        public void CanCallMultiplicationOperator()
        {
            // Arrange
            var vector = new GraphVector(10, 10);
            var scalar = 2;

            // Act
            var result = vector * scalar;

            // Assert
            result.Should().BeEquivalentTo(new GraphVector(20, 20));
        }

        [Fact]
        public void CanCallDivisionOperator()
        {
            // Arrange
            var vector = new GraphVector(10, 10);
            var scalar = 2;

            // Act
            var result = vector / scalar;

            // Assert
            result.Should().BeEquivalentTo(new GraphVector(5, 5));
        }

        [Fact]
        public void XIsInitializedCorrectly()
        {
            _testClass.X.Should().Be(_x);
        }

        [Fact]
        public void CanSetAndGetX()
        {
            // Arrange
            var testValue = 70276152.87;

            // Act
            _testClass.X = testValue;

            // Assert
            _testClass.X.Should().Be(testValue);
        }

        [Fact]
        public void YIsInitializedCorrectly()
        {
            _testClass.Y.Should().Be(_y);
        }

        [Fact]
        public void CanSetAndGetY()
        {
            // Arrange
            var testValue = 1595081217.51;

            // Act
            _testClass.Y = testValue;

            // Assert
            _testClass.Y.Should().Be(testValue);
        }

        [Fact]
        public void CanGetLength()
        {
            // Assert
            new GraphVector(10, 25).Length.Should().Be(26.92582403567252);
        }
    }
}