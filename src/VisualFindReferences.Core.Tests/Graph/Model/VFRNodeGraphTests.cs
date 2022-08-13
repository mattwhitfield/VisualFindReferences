namespace VisualFindReferences.Core.Tests.Graph.Model
{
    using VisualFindReferences.Core.Graph.Model;
    using System;
    using Xunit;
    using FluentAssertions;
    using NSubstitute;
    using System.Windows.Media;
    using Microsoft.CodeAnalysis;
    using VisualFindReferences.Core.Graph.ViewModel;

    public class VFRNodeGraphTests
    {
        private class TestVFRNodeGraph : VFRNodeGraph
        {
            public void PublicNodeAdded(Node node)
            {
                base.NodeAdded(node);
            }

            public void PublicNodeRemoved(Node node)
            {
                base.NodeRemoved(node);
            }

            public NodeGraphViewModel PublicCreateViewModel()
            {
                return base.CreateViewModel();
            }
        }

        private TestVFRNodeGraph _testClass;

        public VFRNodeGraphTests()
        {
            _testClass = new TestVFRNodeGraph();
        }

        // TODO - second round testing with actual syntax trees
        //[Fact]
        //public void CanCallNodeAdded()
        //{
        //    // Arrange
        //    var node = new Node(new NodeGraph(), "TestValue2103704166", "TestValue79184209", 407907063.63, 572273735.22, Geometry.Empty, Brushes.Green);

        //    // Act
        //    _testClass.PublicNodeAdded(node);

        //    // Assert
        //    throw new NotImplementedException("Create or modify test");
        //}

        //[Fact]
        //public void CanCallNodeRemoved()
        //{
        //    // Arrange
        //    var node = new Node(new NodeGraph(), "TestValue1751260322", "TestValue1359405183", 1862612830.98, 1782016978.5, Geometry.Empty, Brushes.Gold);

        //    // Act
        //    _testClass.PublicNodeRemoved(node);

        //    // Assert
        //    throw new NotImplementedException("Create or modify test");
        //}

        //[Fact]
        //public void CanCallGetNodeFor()
        //{
        //    // Arrange
        //    var symbol = Substitute.For<ISymbol>();

        //    // Act
        //    var result = _testClass.GetNodeFor(symbol, out var targetNode);

        //    // Assert
        //    throw new NotImplementedException("Create or modify test");
        //}

        [Fact]
        public void CanCallCreateViewModel()
        {
            // Act
            var result = _testClass.PublicCreateViewModel();

            // Assert
            result.Should().BeAssignableTo<VFRNodeGraphViewModel>();
        }
    }
}