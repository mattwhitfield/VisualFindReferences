namespace VisualFindReferences.Core.Tests.Graph.View
{
    using Xunit;
    using FluentAssertions;
    using System.Windows.Controls;
    using VisualFindReferences.Core.Graph.Model;
    using System.Windows.Media;
    using System.Windows;

    public class ContextMenuEventArgsTests
    {
        private Core.Graph.View.ContextMenuEventArgs _testClass;
        private Node _node;
        private Point _viewSpaceMouseLocation;
        private Point _modelSpaceMouseLocation;

        public ContextMenuEventArgsTests()
        {
            _node = new Node(new NodeGraph(), "TestValue336514407", "TestValue272048505", 1001433972.33, 238840921.44, Geometry.Empty, Brushes.Blue);
            _viewSpaceMouseLocation = new Point();
            _modelSpaceMouseLocation = new Point();
            _testClass = new Core.Graph.View.ContextMenuEventArgs(_node, _viewSpaceMouseLocation, _modelSpaceMouseLocation);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new Core.Graph.View.ContextMenuEventArgs(_node, _viewSpaceMouseLocation, _modelSpaceMouseLocation);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void NodeIsInitializedCorrectly()
        {
            _testClass.Node.Should().BeSameAs(_node);
        }

        [Fact]
        public void ViewSpaceMouseLocationIsInitializedCorrectly()
        {
            _testClass.ViewSpaceMouseLocation.Should().Be(_viewSpaceMouseLocation);
        }

        [Fact]
        public void ModelSpaceMouseLocationIsInitializedCorrectly()
        {
            _testClass.ModelSpaceMouseLocation.Should().Be(_modelSpaceMouseLocation);
        }

        [StaFact]
        public void CanSetAndGetContextMenu()
        {
            // Arrange
            var testValue = new ContextMenu();

            // Act
            _testClass.ContextMenu = testValue;

            // Assert
            _testClass.ContextMenu.Should().BeSameAs(testValue);
        }
    }
}