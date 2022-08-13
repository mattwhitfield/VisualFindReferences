namespace VisualFindReferences.Core.Tests.Graph.View
{
    using VisualFindReferences.Core.Graph.View;
    using System;
    using Xunit;
    using System.Windows;
    using FluentAssertions;

    public class NodeViewsContainerTests
    {
        private class TestNodeViewsContainer : NodeViewsContainer
        {
            public void PublicPrepareContainerForItemOverride(DependencyObject element, object item)
            {
                base.PrepareContainerForItemOverride(element, item);
            }

            public DependencyObject PublicGetContainerForItemOverride()
            {
                return base.GetContainerForItemOverride();
            }
        }

        private TestNodeViewsContainer _testClass;

        public NodeViewsContainerTests()
        {
            _testClass = new TestNodeViewsContainer();
        }

        [StaFact]
        public void CanCallPrepareContainerForItemOverride()
        {
            // Arrange
            var element = new NodeView();
            var item = new object();

            // Act
            _testClass.PublicPrepareContainerForItemOverride(element, item);

            // Assert
            element.Style.Should().NotBeNull();
        }

        [StaFact]
        public void CanCallGetContainerForItemOverride()
        {
            // Act
            var result = _testClass.PublicGetContainerForItemOverride();

            // Assert
            result.Should().BeAssignableTo<NodeView>();
        }
    }
}