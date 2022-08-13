namespace VisualFindReferences.Core.Tests.Graph.View
{
    using VisualFindReferences.Core.Graph.View;
    using Xunit;
    using FluentAssertions;
    using System.ComponentModel;
    using VisualFindReferences.Core.Graph.ViewModel;
    using VisualFindReferences.Core.Graph.Model;
    using System.Windows.Media;

    public class NodeViewTests
    {
        private class TestNodeView : NodeView
        {
            public TestNodeView() : base()
            {
            }

            public void PublicViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                base.ViewModelPropertyChanged(sender, e);
            }
        }

        private TestNodeView _testClass;

        public NodeViewTests()
        {
            _testClass = new TestNodeView();
        }

        [StaFact]
        public void CanConstruct()
        {
            // Act
            var instance = new TestNodeView();

            // Assert
            instance.Should().NotBeNull();
        }

        [StaFact]
        public void PropertiesAreSynchronized()
        {
            _testClass.DataContext = new NodeViewModel(new Node(new NodeGraph(), "TestValue989642182", "TestValue1961606371", 1708511526.81, 22972703.49, Geometry.Empty, Brushes.Pink));

            // Act
            _testClass.ViewModel.IsSelected = false;

            // Assert
            _testClass.IsSelected.Should().BeFalse();

            // Act
            _testClass.ViewModel.IsSelected = true;

            // Assert
            _testClass.IsSelected.Should().BeTrue();
        }

        [StaFact]
        public void CanGetViewModel()
        {
            _testClass.DataContext = new NodeViewModel(new Node(new NodeGraph(), "TestValue989642182", "TestValue1961606371", 1708511526.81, 22972703.49, Geometry.Empty, Brushes.Pink));
            // Assert
            _testClass.ViewModel.Should().BeAssignableTo<NodeViewModel>();
        }

        [StaFact]
        public void CanSetAndGetIsSelected()
        {
            // Arrange
            var testValue = false;

            // Act
            _testClass.IsSelected = testValue;

            // Assert
            _testClass.IsSelected.Should().Be(testValue);
        }

        [StaFact]
        public void CanSetAndGetIsHighlighted()
        {
            // Arrange
            var testValue = false;

            // Act
            _testClass.IsHighlighted = testValue;

            // Assert
            _testClass.IsHighlighted.Should().Be(testValue);
        }
    }
}