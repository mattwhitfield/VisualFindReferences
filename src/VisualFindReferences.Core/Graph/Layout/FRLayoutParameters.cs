using System;

namespace VisualFindReferences.Core.Graph.Layout
{
    public class FRLayoutParameters
    {
        public int VertexCount { get; set; }

        public double K => IdealEdgeLength;

        public double InitialTemperature => Math.Sqrt(Math.Pow(IdealEdgeLength, 2) * VertexCount);

        public double IdealEdgeLength { get; set; } = 50;

        public double ConstantOfAttraction => K * AttractionMultiplier;

        public double AttractionMultiplier { get; set; } = 1.2;

        public double ConstantOfRepulsion => Math.Pow(K * RepulsiveMultiplier, 2);

        public double RepulsiveMultiplier { get; set; } = 0.6;

        public int MaxIterations { get; set; } = 200;

        public double Lambda { get; set; } = 0.95;
    }
}