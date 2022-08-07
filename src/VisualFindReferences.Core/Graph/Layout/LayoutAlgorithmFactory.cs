using System;
using System.Collections.Generic;
using VisualFindReferences.Core.Graph.Model;

namespace VisualFindReferences.Core.Graph.Layout
{
    public static class LayoutAlgorithmFactory
    {
        public static LayoutAlgorithm Create(LayoutAlgorithmType algorithmType, NodeGraph visitedGraph, IDictionary<Node, GraphPoint> verticesPositions)
        {
            switch (algorithmType)
            {
                case LayoutAlgorithmType.VerticalBalancedGrid:
                    return new VerticalBalancedGridLayoutAlgorithm(visitedGraph, verticesPositions);
                case LayoutAlgorithmType.HorizontalBalancedGrid:
                    return new HorizontalBalancedGridLayoutAlgorithm(visitedGraph, verticesPositions);
                case LayoutAlgorithmType.ForceDirected:
                    return new FRLayoutAlgorithm(visitedGraph, verticesPositions);
                default:
                    throw new InvalidOperationException("Layout algorithm" + algorithmType.ToString() + " not known.");
            }
        }
    }
}
