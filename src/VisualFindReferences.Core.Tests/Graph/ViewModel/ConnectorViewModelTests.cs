namespace VisualFindReferences.Core.Tests.Graph.ViewModel
{
    using VisualFindReferences.Core.Graph.ViewModel;
    using Xunit;
    using FluentAssertions;
    using VisualFindReferences.Core.Graph.Model;
    using System.Windows.Media;

    public class ConnectorViewModelTests
    {
        private ConnectorViewModel _testClass;
        private Connector _connection;

        public ConnectorViewModelTests()
        {
            _connection = new Connector(new NodeGraph(), new Node(new NodeGraph(), "TestValue2034290369", "TestValue1576106895", 1302495525.54, 511295103.99, Geometry.Empty, Brushes.Orange), new Node(new NodeGraph(), "TestValue1192205192", "TestValue1007686088", 31065696.09, 1480017622.05, Geometry.Empty, Brushes.Blue));
            _testClass = new ConnectorViewModel(_connection);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new ConnectorViewModel(_connection);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanGetModel()
        {
            // Assert
            _testClass.Model.Should().BeSameAs(_connection);
        }

        [Fact]
        public void CanSetAndGetIsHighlighted()
        {
            _testClass.CheckProperty(x => x.IsHighlighted);
        }

        [Fact]
        public void CanSetAndGetIsBidirectional()
        {
            _testClass.CheckProperty(x => x.IsBidirectional);
        }
    }
}