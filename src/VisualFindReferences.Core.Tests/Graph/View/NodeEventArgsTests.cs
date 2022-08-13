namespace VisualFindReferences.Core.Tests.Graph.View
{
    using VisualFindReferences.Core.Graph.View;
    using Xunit;
    using FluentAssertions;
    using VisualFindReferences.Core.Graph.Model;
    using System.Windows.Media;

    public class NodeEventArgsTests
    {
        private NodeEventArgs _testClass;
        private Node _node;

        public NodeEventArgsTests()
        {
            _node = new Node(new NodeGraph(), "TestValue2032715478", "TestValue1805300853", 832265454.24, 1242357590.43, Geometry.Empty, Brushes.Gold);
            _testClass = new NodeEventArgs(_node);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new NodeEventArgs(_node);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void NodeIsInitializedCorrectly()
        {
            _testClass.Node.Should().BeSameAs(_node);
        }
    }
}