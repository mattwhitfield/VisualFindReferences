using System;
using System.Collections.Generic;
using VisualFindReferences.Core.Graph.Model;

namespace VisualFindReferences.Core.Graph.Layout
{
    public static class LayoutAlgorithmFactory
    {
        public static LayoutAlgorithm Create(LayoutAlgorithmType algorithmType, NodeGraph visitedGraph, IDictionary<Node, GraphPoint> verticesPositions)
        {
            IDictionary<Node, GraphPoint> effectivePositions;

            // these algorithms always lay out all nodes
            if (algorithmType == LayoutAlgorithmType.VerticalBalancedGrid || algorithmType == LayoutAlgorithmType.HorizontalBalancedGrid)
            {
                effectivePositions = new Dictionary<Node, GraphPoint>();
                foreach (var node in visitedGraph.Nodes)
                {
                    effectivePositions[node] = new GraphPoint(node.X, node.Y);
                }
            }
            else
            {
                effectivePositions = verticesPositions;
            }

            switch (algorithmType)
            {
                case LayoutAlgorithmType.VerticalBalancedGrid:
                    return new VerticalBalancedGridLayoutAlgorithm(visitedGraph, effectivePositions);
                case LayoutAlgorithmType.HorizontalBalancedGrid:
                    return new HorizontalBalancedGridLayoutAlgorithm(visitedGraph, effectivePositions);
                case LayoutAlgorithmType.ForceDirected:
                    return new FRLayoutAlgorithm(visitedGraph, effectivePositions);
                default:
                    throw new InvalidOperationException("Layout algorithm" + algorithmType.ToString() + " not known.");
            }
        }
    }
}
