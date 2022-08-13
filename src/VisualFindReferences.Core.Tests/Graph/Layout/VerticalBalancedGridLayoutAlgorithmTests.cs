namespace VisualFindReferences.Core.Tests.Graph.Layout
{
    using VisualFindReferences.Core.Graph.Layout;
    using Xunit;
    using FluentAssertions;
    using System.Collections.Generic;
    using VisualFindReferences.Core.Graph.Model;
    using System.Linq;

    public class VerticalBalancedGridLayoutAlgorithmTests
    {
        private VerticalBalancedGridLayoutAlgorithm _testClass;
        private NodeGraph _visitedGraph;
        private IDictionary<Node, GraphPoint> _verticesPositions;

        public VerticalBalancedGridLayoutAlgorithmTests()
        {
            _visitedGraph = ClassModelProvider.TestGraph;
            _verticesPositions = new Dictionary<Node, GraphPoint>();

            foreach (var node in _visitedGraph.Nodes)
            {
                _verticesPositions.Add(node, new GraphPoint(node.X, node.Y));
            }

            _testClass = new VerticalBalancedGridLayoutAlgorithm(_visitedGraph, _verticesPositions);
        }

        [Fact]
        public void CanConstruct()
        {
            // Act
            var instance = new VerticalBalancedGridLayoutAlgorithm(_visitedGraph, _verticesPositions);

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

            laidOut.Should().Be("{-50:-25},{-110:85},{10:85},{-110:195},{10:195}");
        }
    }
}