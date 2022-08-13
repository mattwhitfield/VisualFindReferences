namespace VisualFindReferences.Core.Tests.Graph.Layout
{
    using VisualFindReferences.Core.Graph.Layout;
    using Xunit;
    using FluentAssertions;

    public class GraphPointTests
    {
        private GraphPoint _testClass;
        private double _x;
        private double _y;

        public GraphPointTests()
        {
            _x = 1223699167.35;
            _y = 1513756759.68;
            _testClass = new GraphPoint(_x, _y);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new GraphPoint(_x, _y);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void ImplementsIEquatable_GraphPoint()
        {
            // Arrange
            var same = new GraphPoint(_x, _y);
            var different = new GraphPoint();

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
        public void CanCallEqualsWithPoint1AndPoint2()
        {
            // Arrange
            var point1 = new GraphPoint(10, 10);
            var point2 = new GraphPoint(10, 10);

            // Act
            var result = GraphPoint.Equals(point1, point2);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CanCallEqualsWithObj()
        {
            var obj = new object();
            _testClass.Equals(obj).Should().BeFalse();
            _testClass.Equals((object)new GraphPoint(_x, _y)).Should().BeTrue();
        }

        [Fact]
        public void CanCallEqualsWithOther()
        {
            // Arrange
            var other = new GraphPoint(_x, _y);

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
            result.Should().Be(new GraphPoint(_x, _y).GetHashCode());
        }

        [Fact]
        public void CanCallEqualityOperator()
        {
            (new GraphPoint(5, 1) == new GraphPoint(5, 1)).Should().BeTrue();
            (new GraphPoint(5, 1) == new GraphPoint(5, 2)).Should().BeFalse();
            (new GraphPoint(4, 1) == new GraphPoint(5, 1)).Should().BeFalse();
        }

        [Fact]
        public void CanCallInequalityOperator()
        {
            (new GraphPoint(5, 1) != new GraphPoint(5, 1)).Should().BeFalse();
            (new GraphPoint(5, 1) != new GraphPoint(5, 2)).Should().BeTrue();
            (new GraphPoint(4, 1) != new GraphPoint(5, 1)).Should().BeTrue();
        }

        [Fact]
        public void CanCallPlusOperator()
        {
            // Arrange
            var point = new GraphPoint(4,1);
            var vector = new GraphVector(3,7);

            // Act
            var result = point + vector;

            // Assert
            result.Should().BeEquivalentTo(new GraphPoint(7, 8));
        }

        [Fact]
        public void CanCallMinusWithGraphPointAndGraphVectorOperator()
        {
            // Arrange
            var point = new GraphPoint(10, 11);
            var vector = new GraphVector(3, 5);

            // Act
            var result = point - vector;

            // Assert
            result.Should().BeEquivalentTo(new GraphPoint(7, 6));
        }

        [Fact]
        public void CanCallMinusWithGraphPointAndGraphPointOperator()
        {
            // Arrange
            var point1 = new GraphPoint(10, 11);
            var point2 = new GraphPoint(3, 5);

            // Act
            var result = point1 - point2;

            // Assert
            result.Should().BeEquivalentTo(new GraphVector(7, 6));
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
            var testValue = 1197688150.89;

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
            var testValue = 1287983080.89;

            // Act
            _testClass.Y = testValue;

            // Assert
            _testClass.Y.Should().Be(testValue);
        }
    }
}