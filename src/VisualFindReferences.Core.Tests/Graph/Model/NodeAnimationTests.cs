namespace VisualFindReferences.Core.Tests.Graph.Model
{
    using VisualFindReferences.Core.Graph.Model;
    using Xunit;
    using FluentAssertions;
    using System.Windows.Media;

    public class NodeAnimationTests
    {
        private NodeAnimation _testClass;
        private Node _node;
        private double _endX;
        private double _endY;

        public NodeAnimationTests()
        {
            _node = new Node(new NodeGraph(), "TestValue1881867812", "TestValue966708572", 20, 40, Geometry.Empty, Brushes.Orange);
            _endX = 120;
            _endY = 140;
            _testClass = new NodeAnimation(_node, _endX, _endY);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new NodeAnimation(_node, _endX, _endY);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanCallApply()
        {
            // Arrange
            var factor = 0.5;

            // Act
            _testClass.Apply(factor);

            // Assert
            _node.X.Should().Be(70);
            _node.Y.Should().Be(90);
        }

        [Fact]
        public void NodeIsInitializedCorrectly()
        {
            _testClass.Node.Should().BeSameAs(_node);
        }

        [Fact]
        public void CanGetXAnimation()
        {
            // Assert
            _testClass.XAnimation.As<object>().Should().BeAssignableTo<DoubleValueAnimation>();
        }

        [Fact]
        public void CanGetYAnimation()
        {
            // Assert
            _testClass.YAnimation.As<object>().Should().BeAssignableTo<DoubleValueAnimation>();
        }
    }
}