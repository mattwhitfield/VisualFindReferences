namespace VisualFindReferences.Core.Tests.Graph.Layout
{
    using VisualFindReferences.Core.Graph.Layout;
    using Xunit;
    using FluentAssertions;
    using System.Collections.Generic;
    using VisualFindReferences.Core.Graph.Model;
    using System.Linq;

    public class HorizontalBalancedGridLayoutAlgorithmTests
    {
        private HorizontalBalancedGridLayoutAlgorithm _testClass;
        private NodeGraph _visitedGraph;
        private IDictionary<Node, GraphPoint> _verticesPositions;

        public HorizontalBalancedGridLayoutAlgorithmTests()
        {
            _visitedGraph = ClassModelProvider.TestGraph;
            _verticesPositions = new Dictionary<Node, GraphPoint>();

            foreach (var node in _visitedGraph.Nodes)
            {
                _verticesPositions.Add(node, new GraphPoint(node.X, node.Y));
            }

            _testClass = new HorizontalBalancedGridLayoutAlgorithm(_visitedGraph, _verticesPositions);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new HorizontalBalancedGridLayoutAlgorithm(_visitedGraph, _verticesPositions);

            // Assert
            instance.Should().NotBeNull();
        }

        [Fact]
        public void CanCallLayout()
        {
            // Act
            _testClass.Layout();

            // Assert
            var laidOut = string.Join(",", _testClass.VerticesPositions.Values.Select(n => "{" + n.X + ":" + n.Y + "}").ToArray());

            laidOut.Should().Be("{0:-50},{160:-85},{160:-15},{320:-85},{320:-15}");
        }
    }
}