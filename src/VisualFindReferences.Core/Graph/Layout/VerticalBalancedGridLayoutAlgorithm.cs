using System.Collections.Generic;
using System.Linq;
using VisualFindReferences.Core.Graph.Model;

namespace VisualFindReferences.Core.Graph.Layout
{
    public class VerticalBalancedGridLayoutAlgorithm : BalancedGridLayoutAlgorithm
    {
        public VerticalBalancedGridLayoutAlgorithm(NodeGraph visitedGraph, IDictionary<Node, GraphPoint> verticesPositions) : base(visitedGraph, verticesPositions)
        {
        }

        protected override void ProcessOutputGroups(IDictionary<int, List<Node>> outputGroups)
        {
            var height = outputGroups.Values.SelectMany(x => x).Select(x => x.ViewModel.View?.ActualHeight ?? 50).Max();
            const int xGap = 20;
            const int yGap = 60;

            var currentY = 0d;
            foreach (var pair in outputGroups.OrderBy(x => x.Key))
            {
                var groupNodes = pair.Value;
                var groupWidth = groupNodes.Sum(x => x.ViewModel.View?.ActualWidth ?? 100) + (groupNodes.Count - 1) * xGap;
                var currentX = groupWidth / -2;

                foreach (Node node in groupNodes)
                {
                    var nodeWidth = node.ViewModel.View?.ActualWidth ?? 100;
                    var nodeHeight = node.ViewModel.View?.ActualHeight ?? 100;
                    var nodeX = currentX;
                    var nodeY = currentY + (height - nodeHeight) / 2;
                    VerticesPositions[node] = new GraphPoint(nodeX, nodeY);

                    currentX += nodeWidth + xGap;
                }

                currentY += height + yGap;
            }
        }
    }
}
