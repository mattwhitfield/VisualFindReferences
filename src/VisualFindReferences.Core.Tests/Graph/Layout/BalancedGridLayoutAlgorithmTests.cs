namespace VisualFindReferences.Core.Tests.Graph.Layout
{
    using VisualFindReferences.Core.Graph.Layout;
    using Xunit;
    using FluentAssertions;
    using NSubstitute;
    using System.Collections.Generic;
    using VisualFindReferences.Core.Graph.Model;

    public class BalancedGridLayoutAlgorithmTests
    {
        private class TestBalancedGridLayoutAlgorithm : BalancedGridLayoutAlgorithm
        {
            public TestBalancedGridLayoutAlgorithm(NodeGraph visitedGraph, IDictionary<Node, GraphPoint> verticesPositions) : base(visitedGraph, verticesPositions)
            {
            }

            public void PublicInitialize()
            {
                base.Initialize();
            }

            public void PublicInternalCompute()
            {
                base.InternalCompute();
            }

            public bool PublicAlgorithmReqiuresOverlapRemoval => base.AlgorithmReqiuresOverlapRemoval;

            protected override void ProcessOutputGroups(IDictionary<int, List<Node>> outputGroups)
            {
            }
        }

        private TestBalancedGridLayoutAlgorithm _testClass;
        private NodeGraph _visitedGraph;
        private IDictionary<Node, GraphPoint> _verticesPositions;

        public BalancedGridLayoutAlgorithmTests()
        {
            _visitedGraph = new NodeGraph();
            _verticesPositions = Substitute.For<IDictionary<Node, GraphPoint>>();
            _testClass = new TestBalancedGridLayoutAlgorithm(_visitedGraph, _verticesPositions);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new TestBalancedGridLayoutAlgorithm(_visitedGraph, _verticesPositions);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanGetPublicAlgorithmReqiuresOverlapRemoval()
        {
            // Assert
            _testClass.PublicAlgorithmReqiuresOverlapRemoval.Should().BeFalse();
        }
    }
}