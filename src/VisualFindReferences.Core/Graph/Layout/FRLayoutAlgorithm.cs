using System;
using System.Collections.Generic;
using VisualFindReferences.Core.Graph.Model;

namespace VisualFindReferences.Core.Graph.Layout
{
    public class FRLayoutAlgorithm
    {
        private double _temperature;

        private double _maxWidth = double.PositiveInfinity;
        private double _maxHeight = double.PositiveInfinity;

        private FlowChart _visitedGraph;

        private FRLayoutParameters _parameters = new FRLayoutParameters();

        public FRLayoutAlgorithm(FlowChart visitedGraph, IDictionary<Node, Point> verticesPositions)
        {
            _visitedGraph = visitedGraph;
            VerticesPositions = verticesPositions;
        }

        private double _constantOfRepulsion;
        private double _constantOfAttraction;

        public IDictionary<Node, Point> VerticesPositions { get; }

        private Random _random = new Random();

        public void Initialize()
        {
            // Initialize with random position
            foreach (var vertex in _visitedGraph.Nodes)
            {
                // For vertices without assigned position
                if (!VerticesPositions.ContainsKey(vertex))
                {
                    VerticesPositions.Add(
                        vertex,
                        new Point(
                            Math.Max(double.Epsilon, _random.NextDouble() * 10),
                            Math.Max(double.Epsilon, _random.NextDouble() * 10)));
                }
            }

            _parameters.VertexCount = _visitedGraph.Nodes.Count;
            _constantOfRepulsion = _parameters.ConstantOfRepulsion;
            _constantOfAttraction = _parameters.ConstantOfAttraction;
        }

        public void InternalCompute()
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

        protected void IterateOne()
        {
            // Create the forces (zero forces)
            var forces = new Dictionary<Node, Vector>();

            foreach (Node v in _visitedGraph.Nodes)
            {
                var force = default(Vector);

                Point posV = VerticesPositions[v];
                foreach (Node u in _visitedGraph.Nodes)
                {
                    // Doesn't repulse itself
                    if (EqualityComparer<Node>.Default.Equals(u, v))
                        continue;

                    // Calculate repulsive force
                    Vector delta = posV - VerticesPositions[u];
                    double length = Math.Max(delta.Length, double.Epsilon);
                    delta = delta / length * _constantOfRepulsion / length;

                    force += delta;
                }

                forces[v] = force;
            }

            foreach (Connector edge in _visitedGraph.Connectors)
            {
                Node source = edge.StartNode;
                Node target = edge.EndNode;

                // Compute attraction point between 2 vertices
                Vector delta = VerticesPositions[source] - VerticesPositions[target];
                double length = Math.Max(delta.Length, double.Epsilon);
                delta = delta / length * Math.Pow(length, 2) / _constantOfAttraction;

                forces[source] -= delta;
                forces[target] += delta;
            }

            foreach (Node vertex in _visitedGraph.Nodes)
            {
                Point position = VerticesPositions[vertex];

                Vector delta = forces[vertex];
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
    }
}