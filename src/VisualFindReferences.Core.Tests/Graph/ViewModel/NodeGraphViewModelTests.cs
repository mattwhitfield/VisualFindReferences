namespace VisualFindReferences.Core.Tests.Graph.ViewModel
{
    using VisualFindReferences.Core.Graph.ViewModel;
    using Xunit;
    using FluentAssertions;
    using VisualFindReferences.Core.Graph.Model;
    using System.Collections.ObjectModel;
    using VisualFindReferences.Core.Graph.Layout;

    public class NodeGraphViewModelTests
    {
        private class TestNodeGraphViewModel : NodeGraphViewModel
        {
            public TestNodeGraphViewModel(NodeGraph nodeGraph) : base(nodeGraph)
            {
            }

            public void PublicHandleContinuation<T>(T result)
            {
                base.HandleContinuation<T>(result);
            }
        }

        private TestNodeGraphViewModel _testClass;
        private NodeGraph _nodeGraph;

        public NodeGraphViewModelTests()
        {
            _nodeGraph = new NodeGraph();
            _testClass = new TestNodeGraphViewModel(_nodeGraph);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new TestNodeGraphViewModel(_nodeGraph);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanGetModel()
        {
            // Assert
            _testClass.Model.Should().BeAssignableTo<NodeGraph>();
        }

        [Fact]
        public void CanGetNodeViewModels()
        {
            // Assert
            _testClass.NodeViewModels.Should().BeAssignableTo<ObservableCollection<NodeViewModel>>();
        }

        [Fact]
        public void CanGetConnectorViewModels()
        {
            // Assert
            _testClass.ConnectorViewModels.Should().BeAssignableTo<ObservableCollection<ConnectorViewModel>>();
        }

        [Fact]
        public void CanSetAndGetLayoutType()
        {
            _testClass.CheckProperty(x => x.LayoutType, LayoutAlgorithmType.VerticalBalancedGrid, LayoutAlgorithmType.ForceDirected);
        }

        [Fact]
        public void CanSetAndGetIsBusy()
        {
            _testClass.CheckProperty(x => x.IsBusy);
        }

        [Fact]
        public void CanSetAndGetBusyText()
        {
            _testClass.CheckProperty(x => x.BusyText);
        }
    }
}