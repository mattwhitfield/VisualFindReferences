using System;
using System.Collections.Generic;
using VisualFindReferences.Core.Graph.Model;

namespace VisualFindReferences.Core.Graph.Layout
{
    public class FRLayoutAlgorithm : LayoutAlgorithm
    {
        private double _temperature;

        private double _maxWidth = double.PositiveInfinity;
        private double _maxHeight = double.PositiveInfinity;

        private FRLayoutParameters _parameters = new FRLayoutParameters();

        public FRLayoutAlgorithm(NodeGraph visitedGraph, IDictionary<Node, GraphPoint> verticesPositions) : base(visitedGraph, verticesPositions)
        { }

        private double _constantOfRepulsion;
        private double _constantOfAttraction;

        protected override bool AlgorithmReqiuresOverlapRemoval => true;

        protected override void Initialize()
        {
            // Initialize with random position
            foreach (var vertex in VisitedGraph.Nodes)
            {
                // For vertices without assigned position
                if (!VerticesPositions.ContainsKey(vertex))
                {
                    VerticesPositions.Add(
                        vertex,
                        new GraphPoint(
                            Math.Max(double.Epsilon, Random.NextDouble() * 10),
                            Math.Max(double.Epsilon, Random.NextDouble() * 10)));
                }
            }

            _parameters.VertexCount = VisitedGraph.Nodes.Count;
            _constantOfRepulsion = _parameters.ConstantOfRepulsion;
            _constantOfAttraction = _parameters.ConstantOfAttraction;
        }

        protected override void InternalCompute()
        {
            // Actual temperature of the 'mass'. Used for cooling.
            _temperature = _parameters.InitialTemperature;
            double minimalTemperature = _temperature * 0.01;
            for (int i = 0; i < _parameters.MaxIterations && _temperature > minimalTemperature; ++i)
            {
                IterateOne();

                // Make some cooling
                _temperature *= _parameters.Lambda;
            }
        }

        private void IterateOne()
        {
            // Create the forces (zero forces)
            var forces = new Dictionary<Node, GraphVector>();

            foreach (Node v in VisitedGraph.Nodes)
            {
                var force = default(GraphVector);

                GraphPoint posV = VerticesPositions[v];
                foreach (Node u in VisitedGraph.Nodes)
                {
                    // Doesn't repulse itself
                    if (EqualityComparer<Node>.Default.Equals(u, v))
                        continue;

                    // Calculate repulsive force
                    GraphVector delta = posV - VerticesPositions[u];
                    double length = Math.Max(delta.Length, double.Epsilon);
                    delta = delta / length * _constantOfRepulsion / length;

                    force += delta;
                }

                forces[v] = GrandmaProtection(force);
            }

            foreach (Connector edge in VisitedGraph.Connectors)
            {
                Node source = edge.StartNode;
                Node target = edge.EndNode;

                // Compute attraction point between 2 vertices
                GraphVector delta = VerticesPositions[source] - VerticesPositions[target];
                double length = Math.Max(delta.Length, double.Epsilon);
                delta = delta / length * Math.Pow(length, 2) / _constantOfAttraction;

                var safeDelta = GrandmaProtection(delta);
                forces[source] -= safeDelta;
                forces[target] += safeDelta;
            }

            foreach (Node vertex in VisitedGraph.Nodes)
            {
                GraphPoint position = VerticesPositions[vertex];

                GraphVector delta = forces[vertex];
                if (delta != default)
                {
                    double length = Math.Max(delta.Length, double.Epsilon);
                    delta = delta / length * Math.Min(length, _temperature);

                    position += delta;

                    // Ensure bounds
                    position.X = Math.Min(_maxWidth, Math.Max(0, position.X));
                    position.Y = Math.Min(_maxHeight, Math.Max(0, position.Y));
                    VerticesPositions[vertex] = position;
                }
            }
        }

        private GraphVector GrandmaProtection(GraphVector input)
        {
            var xIsYourNan = double.IsNaN(input.X);
            var yIsYourNan = double.IsNaN(input.Y);

            if (xIsYourNan || yIsYourNan)
            {
                input = new GraphVector(xIsYourNan ? 0 : input.X, yIsYourNan ? 0 : input.Y);
            }

            return input;
        }
    }
}