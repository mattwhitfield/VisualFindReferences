namespace VisualFindReferences.Core.Tests.Graph.Model
{
    using VisualFindReferences.Core.Graph.Model;
    using Xunit;
    using FluentAssertions;
    using VisualFindReferences.Core.Graph.ViewModel;
    using System.Windows.Media;

    public class NodeTests
    {
        private Node _testClass;
        private NodeGraph _nodeGraph;
        private string _containerName;
        private string _typeName;
        private double _x;
        private double _y;
        private Geometry _icon;
        private Brush _iconColor;

        public NodeTests()
        {
            _nodeGraph = new NodeGraph();
            _containerName = "TestValue1833202491";
            _typeName = "TestValue1774635624";
            _x = 1558443445.02;
            _y = 6569323.2;
            _icon = Geometry.Empty;
            _iconColor = Brushes.Brown;
            _testClass = new Node(_nodeGraph, _containerName, _typeName, _x, _y, _icon, _iconColor);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new Node(_nodeGraph, _containerName, _typeName, _x, _y, _icon, _iconColor);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanGetOwner()
        {
            // Assert
            _testClass.Owner.Should().BeSameAs(_nodeGraph);
        }

        [Fact]
        public void CanGetViewModel()
        {
            // Assert
            _testClass.ViewModel.Should().BeAssignableTo<NodeViewModel>();
        }

        [Fact]
        public void ContainerNameIsInitializedCorrectly()
        {
            _testClass.ContainerName.Should().Be(_containerName);
        }

        [Fact]
        public void TypeNameIsInitializedCorrectly()
        {
            _testClass.TypeName.Should().Be(_typeName);
        }

        [Fact]
        public void XIsInitializedCorrectly()
        {
            _testClass.X.Should().Be(_x);
        }

        [Fact]
        public void CanSetAndGetX()
        {
            _testClass.CheckProperty(x => x.X, 335472964.2, 482625811.8);
        }

        [Fact]
        public void YIsInitializedCorrectly()
        {
            _testClass.Y.Should().Be(_y);
        }

        [Fact]
        public void CanSetAndGetY()
        {
            _testClass.CheckProperty(x => x.Y, 2014638280.3799999, 1211414497.92);
        }

        [Fact]
        public void IconIsInitializedCorrectly()
        {
            _testClass.Icon.Should().BeSameAs(_icon);
        }

        [Fact]
        public void IconColorIsInitializedCorrectly()
        {
            _testClass.IconColor.Should().BeSameAs(_iconColor);
        }
    }
}