using System.Collections.Generic;
using System.Linq;
using VisualFindReferences.Core.Graph.Model;

namespace VisualFindReferences.Core.Graph.Layout
{
    public class HorizontalBalancedGridLayoutAlgorithm : BalancedGridLayoutAlgorithm
    {
        public HorizontalBalancedGridLayoutAlgorithm(NodeGraph visitedGraph, IDictionary<Node, Point> verticesPositions) : base(visitedGraph, verticesPositions)
        {
        }

        protected override void ProcessOutputGroups(IDictionary<int, List<Node>> outputGroups)
        {
            var height = outputGroups.Values.SelectMany(x => x).Select(x => x.ViewModel.View?.ActualHeight ?? 50).Max();
            const int xGap = 60;
            const int yGap = 20;

            var currentX = 0d;
            foreach (var pair in outputGroups.OrderBy(x => x.Key))
            {
                var groupNodes = pair.Value;
                var groupWidth = groupNodes.Select(x => x.ViewModel.View?.ActualWidth ?? 100).Max();
                var currentGroupHeight = groupNodes.Count * height + (groupNodes.Count - 1) * yGap;
                var currentY = currentGroupHeight / -2;

                foreach (Node node in groupNodes)
                {
                    var nodeWidth = node.ViewModel.View?.ActualWidth ?? 100;
                    var nodeHeight = node.ViewModel.View?.ActualHeight ?? 100;
                    var nodeX = currentX + (groupWidth - nodeWidth) / 2;
                    var nodeY = currentY + (height - nodeHeight) / 2;
                    VerticesPositions[node] = new Point(nodeX, nodeY);

                    currentY += height + yGap;
                }

                currentX += groupWidth + xGap;
            }
        }
    }
}
