namespace VisualFindReferences.Core.Tests.Graph.Layout
{
    using VisualFindReferences.Core.Graph.Layout;
    using System;
    using Xunit;
    using FluentAssertions;
    using NSubstitute;
    using VisualFindReferences.Core.Graph.Model;
    using System.Collections.Generic;

    public static class LayoutAlgorithmFactoryTests
    {
        [Theory]
        [InlineData(LayoutAlgorithmType.VerticalBalancedGrid, typeof(VerticalBalancedGridLayoutAlgorithm))]
        [InlineData(LayoutAlgorithmType.HorizontalBalancedGrid, typeof(HorizontalBalancedGridLayoutAlgorithm))]
        [InlineData(LayoutAlgorithmType.ForceDirected, typeof(FRLayoutAlgorithm))]
        public static void CanCallCreate(LayoutAlgorithmType algorithmType, Type expectedType)
        {
            // Arrange
            var visitedGraph = new NodeGraph();
            var verticesPositions = Substitute.For<IDictionary<Node, GraphPoint>>();

            // Act
            var result = LayoutAlgorithmFactory.Create(algorithmType, visitedGraph, verticesPositions);

            // Assert
            result.Should().BeOfType(expectedType);
        }
    }
}