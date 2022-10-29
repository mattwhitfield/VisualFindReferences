namespace VisualFindReferences.Core.Tests.Graph.ViewModel
{
    using VisualFindReferences.Core.Graph.ViewModel;
    using Xunit;
    using FluentAssertions;
    using VisualFindReferences.Core.Graph.Model;
    using System.Collections.ObjectModel;
    using System.Windows.Media;

    public class NodeViewModelTests
    {
        private NodeViewModel _testClass;
        private Node _node;

        public NodeViewModelTests()
        {
            _node = new Node(new NodeGraph(), "TestValue989642182", "TestValue1961606371", 1708511526.81, 22972703.49, Geometry.Empty, Brushes.Pink);
            _testClass = new NodeViewModel(_node);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new NodeViewModel(_node);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanGetModel()
        {
            // Assert
            _testClass.Model.Should().BeSameAs(_node);
        }

        [Fact]
        public void CanGetOutboundConnectors()
        {
            // Assert
            _testClass.OutboundConnectors.Should().BeAssignableTo<ObservableCollection<Connector>>();
        }

        [Fact]
        public void CanSetAndGetIsSelected()
        {
            _testClass.CheckProperty(x => x.IsSelected);
        }

        [Fact]
        public void CanSetAndGetIsHighlighted()
        {
            _testClass.CheckProperty(x => x.IsHighlighted);
        }
    }
}