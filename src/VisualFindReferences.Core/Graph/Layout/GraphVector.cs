using System;

namespace VisualFindReferences.Core.Graph.Layout
{
    public struct GraphVector : IEquatable<GraphVector>
    {
        public double X { get; set; }

        public double Y { get; set; }

        public GraphVector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(GraphVector vector1, GraphVector vector2)
        {
            return Equals(vector1, vector2);
        }

        public static bool operator !=(GraphVector vector1, GraphVector vector2)
        {
            return !(vector1 == vector2);
        }

        public static bool Equals(GraphVector vector1, GraphVector vector2)
        {
            return MathUtils.NearEqual(vector1.X, vector2.X) && MathUtils.NearEqual(vector1.Y, vector2.Y);
        }

        public override bool Equals(object obj)
        {
            return obj is GraphVector vector && Equals(this, vector);
        }

        public bool Equals(GraphVector other)
        {
            return Equals(this, other);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public double Length => Math.Sqrt(X * X + Y * Y);

        public static GraphVector operator +(GraphVector vector1, GraphVector vector2)
        {
            return new GraphVector(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }

        public static GraphVector operator -(GraphVector vector1, GraphVector vector2)
        {
            return new GraphVector(vector1.X - vector2.X, vector1.Y - vector2.Y);
        }

        public static GraphVector operator *(GraphVector vector, double scalar)
        {
            return new GraphVector(vector.X * scalar, vector.Y * scalar);
        }

        public static GraphVector operator /(GraphVector vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }
    }
}