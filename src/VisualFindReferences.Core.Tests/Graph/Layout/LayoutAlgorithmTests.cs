namespace VisualFindReferences.Core.Tests.Graph.Layout
{
    using VisualFindReferences.Core.Graph.Layout;
    using Xunit;
    using FluentAssertions;
    using NSubstitute;
    using VisualFindReferences.Core.Graph.Model;
    using System.Collections.Generic;

    public class LayoutAlgorithmTests
    {
        private class TestLayoutAlgorithm : LayoutAlgorithm
        {
            public TestLayoutAlgorithm(NodeGraph visitedGraph, IDictionary<Node, GraphPoint> verticesPositions) : base(visitedGraph, verticesPositions)
            {
            }

            public NodeGraph PublicVisitedGraph => base.VisitedGraph;

            public bool PublicAlgorithmReqiuresOverlapRemoval { get; set; }

            protected override void Initialize()
            {
            }

            protected override void InternalCompute()
            {
            }

            protected override bool AlgorithmReqiuresOverlapRemoval => PublicAlgorithmReqiuresOverlapRemoval;
        }

        private TestLayoutAlgorithm _testClass;
        private NodeGraph _visitedGraph;
        private IDictionary<Node, GraphPoint> _verticesPositions;

        public LayoutAlgorithmTests()
        {
            _visitedGraph = new NodeGraph();
            _verticesPositions = Substitute.For<IDictionary<Node, GraphPoint>>();
            _testClass = new TestLayoutAlgorithm(_visitedGraph, _verticesPositions);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new TestLayoutAlgorithm(_visitedGraph, _verticesPositions);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanGetPublicVisitedGraph()
        {
            // Assert
            _testClass.PublicVisitedGraph.Should().BeSameAs(_visitedGraph);
        }

        [Fact]
        public void VerticesPositionsIsInitializedCorrectly()
        {
            _testClass.VerticesPositions.Should().BeSameAs(_verticesPositions);
        }
    }
}