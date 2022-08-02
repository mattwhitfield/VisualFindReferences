using System;
using System.Collections.Generic;
using VisualFindReferences.Core.Graph.Model;

namespace VisualFindReferences.Core.Graph.Layout
{
    public abstract class LayoutAlgorithm
    {
        public LayoutAlgorithm(NodeGraph visitedGraph, IDictionary<Node, Point> verticesPositions)
        {
            VisitedGraph = visitedGraph;
            VerticesPositions = verticesPositions;
        }

        protected NodeGraph VisitedGraph;
        public IDictionary<Node, Point> VerticesPositions { get; }

        protected Random Random = new Random();

        public void Layout()
        {
            Initialize();
            InternalCompute();
        }

        protected abstract void Initialize();

        protected abstract void InternalCompute();
    }
}