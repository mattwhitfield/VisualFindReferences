namespace VisualFindReferences.Core.Tests.Graph.Model
{
    using VisualFindReferences.Core.Graph.Model;
    using Xunit;
    using FluentAssertions;
    using VisualFindReferences.Core.Graph.ViewModel;
    using System.Windows.Media;

    public class ConnectorTests
    {
        private Connector _testClass;
        private NodeGraph _nodeGraph;
        private Node _startNode;
        private Node _endNode;

        public ConnectorTests()
        {
            _nodeGraph = new NodeGraph();
            _startNode = new Node(new NodeGraph(), "TestValue1621437443", "TestValue1439008545", 423196358.31, 190494652.59, Geometry.Empty, Brushes.White);
            _endNode = new Node(new NodeGraph(), "TestValue1229554943", "TestValue384105997", 504952715.52, 1625612983.83, Geometry.Empty, Brushes.Cyan);
            _testClass = new Connector(_nodeGraph, _startNode, _endNode);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new Connector(_nodeGraph, _startNode, _endNode);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void NodeGraphIsInitializedCorrectly()
        {
            _testClass.NodeGraph.Should().BeSameAs(_nodeGraph);
        }

        [Fact]
        public void CanGetViewModel()
        {
            // Assert
            _testClass.ViewModel.Should().BeAssignableTo<ConnectorViewModel>();
        }

        [Fact]
        public void StartNodeIsInitializedCorrectly()
        {
            _testClass.StartNode.Should().BeSameAs(_startNode);
        }

        [Fact]
        public void EndNodeIsInitializedCorrectly()
        {
            _testClass.EndNode.Should().BeSameAs(_endNode);
        }
    }
}