namespace VisualFindReferences.Core.Tests.Graph.View
{
    using VisualFindReferences.Core.Graph.View;
    using Xunit;
    using FluentAssertions;
    using VisualFindReferences.Core.Graph.Model;
    using System.Windows.Media;
    using System.Windows;

    public class NodeGraphViewTests
    {
        private class TestNodeGraphView : NodeGraphView
        {
            public TestNodeGraphView() : base()
            {
            }
        }

        private TestNodeGraphView _testClass;

        public NodeGraphViewTests()
        {
            _testClass = new TestNodeGraphView();
            var graph = new NodeGraph();
            var node = new Node(graph, "TestValue1721531683", "TestValue1830838990", 431162518.05, 1486573164.45, Geometry.Empty, System.Windows.Media.Brushes.Lime);
            graph.Nodes.Add(node);
            _testClass.DataContext = graph.ViewModel;
        }

        [StaFact]
        public void CanConstruct()
        {
            // Act
            var instance = new TestNodeGraphView();

            // Assert
            instance.Should().NotBeNull();
        }

        [StaFact]
        public void CanCallSelectAllNodesAndDeselectAllNodes()
        {
            // Act
            _testClass.SelectAllNodes();

            // Assert
            _testClass.ViewModel.NodeViewModels.Should().OnlyContain(x => x.IsSelected);

            // Act
            _testClass.DeselectAllNodes();

            // Assert
            _testClass.ViewModel.NodeViewModels.Should().OnlyContain(x => !x.IsSelected);
        }

        [StaFact]
        public void CanCallOnApplyTemplate()
        {
            // Act
            FluentActions.Invoking(() => _testClass.OnApplyTemplate()).Should().NotThrow();
        }

        [StaFact]
        public void CanGetZoomAndPan()
        {
            // Assert
            _testClass.ZoomAndPan.Should().BeAssignableTo<ZoomAndPan>();
        }

        [StaFact]
        public void CanSetAndGetSelectionWidth()
        {
            // Arrange
            var testValue = 1335561309.72;

            // Act
            _testClass.SelectionWidth = testValue;

            // Assert
            _testClass.SelectionWidth.Should().Be(testValue);
        }

        [StaFact]
        public void CanSetAndGetSelectionHeight()
        {
            // Arrange
            var testValue = 19018367.28;

            // Act
            _testClass.SelectionHeight = testValue;

            // Assert
            _testClass.SelectionHeight.Should().Be(testValue);
        }

        [StaFact]
        public void CanSetAndGetSelectionStartY()
        {
            // Arrange
            var testValue = 412305153.48;

            // Act
            _testClass.SelectionStartY = testValue;

            // Assert
            _testClass.SelectionStartY.Should().Be(testValue);
        }

        [StaFact]
        public void CanSetAndGetSelectionStartX()
        {
            // Arrange
            var testValue = 1950697444.41;

            // Act
            _testClass.SelectionStartX = testValue;

            // Assert
            _testClass.SelectionStartX.Should().Be(testValue);
        }

        [StaFact]
        public void CanSetAndGetSelectionVisibility()
        {
            // Arrange
            var testValue = Visibility.Visible;

            // Act
            _testClass.SelectionVisibility = testValue;

            // Assert
            _testClass.SelectionVisibility.Should().Be(testValue);
        }
    }
}