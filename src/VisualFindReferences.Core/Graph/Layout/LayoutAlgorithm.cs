using System;
using System.Collections.Generic;
using System.Linq;
using VisualFindReferences.Core.Graph.Model;

namespace VisualFindReferences.Core.Graph.Layout
{
    public abstract class LayoutAlgorithm
    {
        public LayoutAlgorithm(NodeGraph visitedGraph, IDictionary<Node, GraphPoint> verticesPositions)
        {
            VisitedGraph = visitedGraph;
            VerticesPositions = verticesPositions;
        }

        protected NodeGraph VisitedGraph { get; }
        public IDictionary<Node, GraphPoint> VerticesPositions { get; }

        protected Random Random = new Random();

        protected abstract bool AlgorithmReqiuresOverlapRemoval { get; }

        public void Layout()
        {
            Initialize();
            InternalCompute();

            if (AlgorithmReqiuresOverlapRemoval && VerticesPositions.Keys.All(x => x.ViewModel.View != null))
            {
                var fsaDictionary = new Dictionary<Node, GraphRect>();
                foreach (var pair in VerticesPositions)
                {
                    var view = pair.Key.ViewModel.View;
                    if (view != null)
                    {
                        fsaDictionary[pair.Key] = new GraphRect(pair.Value.X, pair.Value.Y, view.ActualWidth, view.ActualHeight);
                    }
                }

                var overlapRemoval = new FSAAlgorithm(fsaDictionary);
                overlapRemoval.InternalCompute();

                foreach (var pair in fsaDictionary)
                {
                    VerticesPositions[pair.Key] = new GraphPoint(pair.Value.X, pair.Value.Y);
                }
            }
        }

        protected abstract void Initialize();

        protected abstract void InternalCompute();
    }
}