namespace VisualFindReferences.Core.Tests.Graph.Model
{
    using VisualFindReferences.Core.Graph.Model;
    using Xunit;
    using FluentAssertions;
    using VisualFindReferences.Core.Graph.ViewModel;
    using System.Collections.ObjectModel;

    public class NodeGraphTests
    {
        private class TestNodeGraph : NodeGraph
        {
            public TestNodeGraph() : base()
            {
            }

            public NodeGraphViewModel PublicCreateViewModel()
            {
                return base.CreateViewModel();
            }

            public void PublicNodeAdded(Node node)
            {
                base.NodeAdded(node);
            }

            public void PublicNodeRemoved(Node node)
            {
                base.NodeRemoved(node);
            }

            public void PublicConnectorAdded(Connector connector)
            {
                base.ConnectorAdded(connector);
            }

            public void PublicConnectorRemoved(Connector connector)
            {
                base.ConnectorRemoved(connector);
            }
        }

        private TestNodeGraph _testClass;

        public NodeGraphTests()
        {
            _testClass = new TestNodeGraph();
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new TestNodeGraph();

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanGetViewModel()
        {
            // Assert
            _testClass.ViewModel.Should().BeAssignableTo<NodeGraphViewModel>();
        }

        [Fact]
        public void CanGetNodes()
        {
            // Assert
            _testClass.Nodes.Should().BeAssignableTo<ObservableCollection<Node>>();
        }

        [Fact]
        public void CanGetConnectors()
        {
            // Assert
            _testClass.Connectors.Should().BeAssignableTo<ObservableCollection<Connector>>();
        }
    }
}