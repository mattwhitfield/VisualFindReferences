namespace VisualFindReferences.Core.Tests.Graph.Model
{
    using VisualFindReferences.Core.Graph.Model;
    using Xunit;
    using FluentAssertions;
    using System.Collections.Generic;
    using VisualFindReferences.Core.Graph.View;
    using System.Windows.Media;

    public class GraphAnimationTests
    {
        private GraphAnimation _testClass;
        private ZoomAndPan _target;
        private double _targetScale;
        private double _targetStartX;
        private double _targetStartY;
        private Node _node;
        private double _endX;
        private double _endY;

        public GraphAnimationTests()
        {
            _target = new ZoomAndPan();
            _targetScale = 3;
            _targetStartX = 300;
            _targetStartY = 700;
            _testClass = new GraphAnimation(_target, _targetScale, _targetStartX, _targetStartY);

            _node = new Node(new NodeGraph(), "TestValue1881867812", "TestValue966708572", 20, 40, Geometry.Empty, Brushes.Orange);
            _endX = 120;
            _endY = 140;
            _testClass.NodeAnimations.Add(new NodeAnimation(_node, _endX, _endY));
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new GraphAnimation(_target, _targetScale, _targetStartX, _targetStartY);

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
            _target.Scale.Should().Be(2);
            _target.StartX.Should().Be(150);
            _target.StartY.Should().Be(350);
        }

        [Fact]
        public void CanGetNodeAnimations()
        {
            // Assert
            _testClass.NodeAnimations.Should().BeAssignableTo<IList<NodeAnimation>>();
        }

        [Fact]
        public void CanGetScaleAnimation()
        {
            // Assert
            _testClass.ScaleAnimation.As<object>().Should().BeAssignableTo<DoubleValueAnimation>();
            _testClass.ScaleAnimation.Lerp(0.5).Should().Be(_target.Scale + (_targetScale - _target.Scale) / 2.0);
        }

        [Fact]
        public void CanGetStartXAnimation()
        {
            // Assert
            _testClass.StartXAnimation.As<object>().Should().BeAssignableTo<DoubleValueAnimation>();
            _testClass.StartXAnimation.Lerp(0.5).Should().Be(_target.StartX + (_targetStartX - _target.StartX) / 2.0);
        }

        [Fact]
        public void CanGetStartYAnimation()
        {
            // Assert
            _testClass.StartYAnimation.As<object>().Should().BeAssignableTo<DoubleValueAnimation>();
            _testClass.StartYAnimation.Lerp(0.5).Should().Be(_target.StartY + (_targetStartY - _target.StartY) / 2.0);
        }

        [Fact]
        public void TargetIsInitializedCorrectly()
        {
            _testClass.Target.Should().BeSameAs(_target);
        }
    }
}