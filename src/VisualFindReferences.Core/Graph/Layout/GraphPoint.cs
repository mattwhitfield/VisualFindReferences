using System;

namespace VisualFindReferences.Core.Graph.Layout
{
    public struct GraphPoint : IEquatable<GraphPoint>
    {
        public double X { get; set; }

        public double Y { get; set; }

        public GraphPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(GraphPoint point1, GraphPoint point2)
        {
            return Equals(point1, point2);
        }

        public static bool operator !=(GraphPoint point1, GraphPoint point2)
        {
            return !(point1 == point2);
        }

        public static bool Equals(GraphPoint point1, GraphPoint point2)
        {
            return MathUtils.NearEqual(point1.X, point2.X) && MathUtils.NearEqual(point1.Y, point2.Y);
        }

        public override bool Equals(object obj)
        {
            return obj is GraphPoint point && Equals(this, point);
        }

        public bool Equals(GraphPoint other)
        {
            return Equals(this, other);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public static GraphPoint operator +(GraphPoint point, GraphVector vector)
        {
            return new GraphPoint(point.X + vector.X, point.Y + vector.Y);
        }

        public static GraphPoint operator -(GraphPoint point, GraphVector vector)
        {
            return new GraphPoint(point.X - vector.X, point.Y - vector.Y);
        }

        public static GraphVector operator -(GraphPoint point1, GraphPoint point2)
        {
            return new GraphVector(point1.X - point2.X, point1.Y - point2.Y);
        }
    }
}