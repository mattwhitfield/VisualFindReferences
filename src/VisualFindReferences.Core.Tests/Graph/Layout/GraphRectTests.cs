namespace VisualFindReferences.Core.Tests.Graph.Layout
{
    using VisualFindReferences.Core.Graph.Layout;
    using Xunit;
    using FluentAssertions;

    public class GraphRectTests
    {
        private GraphRect _testClass;
        private double _x;
        private double _y;
        private double _width;
        private double _height;

        public GraphRectTests()
        {
            _x = 1245758388.93;
            _y = 449200326.96;
            _width = 768853535.67;
            _height = 1520762641.2;
            _testClass = new GraphRect(_x, _y, _width, _height);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new GraphRect(_x, _y, _width, _height);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void ImplementsIEquatable_GraphRect()
        {
            // Arrange
            var same = new GraphRect(_x, _y, _width, _height);
            var different = new GraphRect();

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
        public void CanCallGetCenter()
        {
            // Act
            var result = new GraphRect(1, 11, 10, 20).GetCenter();

            // Assert
            result.Should().BeEquivalentTo(new GraphPoint(6, 21));
        }

        [Fact]
        public void CanCallEqualsWithRect1AndRect2()
        {
            // Arrange
            var rect1 = new GraphRect(1, 2, 3, 4);
            var rect2 = new GraphRect(1, 2, 3, 4);

            // Act
            var result = GraphRect.Equals(rect1, rect2);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CanCallEqualsWithObj()
        {
            // Arrange
            var obj = new object();

            // Act
            var result = _testClass.Equals(obj);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void CanCallEqualsWithOther()
        {
            // Arrange
            var other = new GraphRect(_x, _y, _width, _height);

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
            result.Should().Be(new GraphRect(_x, _y, _width, _height).GetHashCode());
        }

        [Fact]
        public void CanCallIntersectsWith()
        {
            new GraphRect(10, 10, 10, 10).IntersectsWith(new GraphRect(30, 30, 5, 5)).Should().BeFalse();
            new GraphRect(10, 10, 10, 10).IntersectsWith(new GraphRect(17, 12, 5, 5)).Should().BeTrue();
            new GraphRect(10, 10, 10, 10).IntersectsWith(new GraphRect(7, 12, 5, 5)).Should().BeTrue();
            new GraphRect(10, 10, 10, 10).IntersectsWith(new GraphRect(12, 17, 5, 5)).Should().BeTrue();
            new GraphRect(10, 10, 10, 10).IntersectsWith(new GraphRect(12, 7, 5, 5)).Should().BeTrue();
        }

        [Fact]
        public void CanCallOffset()
        {
            // Act
            _testClass.Offset(3, -3);

            // Assert
            _testClass.X.Should().Be(_x + 3);
            _testClass.Y.Should().Be(_y - 3);
        }

        [Fact]
        public void CanCallEqualityOperator()
        {
            (new GraphRect(1, 2, 3, 4) == new GraphRect(1, 2, 3, 4)).Should().BeTrue();
            (new GraphRect(1, 2, 3, 5) == new GraphRect(1, 2, 3, 4)).Should().BeFalse();
        }

        [Fact]
        public void CanCallInequalityOperator()
        {
            (new GraphRect(1, 2, 3, 4) != new GraphRect(1, 2, 3, 4)).Should().BeFalse();
            (new GraphRect(1, 2, 3, 5) != new GraphRect(1, 2, 3, 4)).Should().BeTrue();
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
            var testValue = 1037437250.85;

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
            var testValue = 1399304122.92;

            // Act
            _testClass.Y = testValue;

            // Assert
            _testClass.Y.Should().Be(testValue);
        }

        [Fact]
        public void WidthIsInitializedCorrectly()
        {
            _testClass.Width.Should().Be(_width);
        }

        [Fact]
        public void CanSetAndGetWidth()
        {
            // Arrange
            var testValue = 1899179446.23;

            // Act
            _testClass.Width = testValue;

            // Assert
            _testClass.Width.Should().Be(testValue);
        }

        [Fact]
        public void HeightIsInitializedCorrectly()
        {
            _testClass.Height.Should().Be(_height);
        }

        [Fact]
        public void CanSetAndGetHeight()
        {
            // Arrange
            var testValue = 1028800872.5;

            // Act
            _testClass.Height = testValue;

            // Assert
            _testClass.Height.Should().Be(testValue);
        }

        [Fact]
        public void CanGetLeft()
        {
            // Assert
            _testClass.Left.Should().Be(_x);
        }

        [Fact]
        public void CanGetTop()
        {
            // Assert
            _testClass.Top.Should().Be(_y);
        }

        [Fact]
        public void CanGetRight()
        {
            // Assert
            _testClass.Right.Should().Be(_x + _width);
        }

        [Fact]
        public void CanGetBottom()
        {
            // Assert
            _testClass.Bottom.Should().Be(_y + _height);
        }
    }
}