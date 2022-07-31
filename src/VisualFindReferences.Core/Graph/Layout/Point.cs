using System;

namespace VisualFindReferences.Core.Graph.Layout
{
    public struct Point : IEquatable<Point>
    {
        public double X { get; set; }

        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Point point1, Point point2)
        {
            return Equals(point1, point2);
        }

        public static bool operator !=(Point point1, Point point2)
        {
            return !(point1 == point2);
        }

        public static bool Equals(Point point1, Point point2)
        {
            return MathUtils.NearEqual(point1.X, point2.X) && MathUtils.NearEqual(point1.Y, point2.Y);
        }

        public override bool Equals(object obj)
        {
            return obj is Point point && Equals(this, point);
        }

        public bool Equals(Point other)
        {
            return Equals(this, other);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public static Point operator +(Point point, Vector vector)
        {
            return new Point(point.X + vector.X, point.Y + vector.Y);
        }

        public static Point operator -(Point point, Vector vector)
        {
            return new Point(point.X - vector.X, point.Y - vector.Y);
        }

        public static Vector operator -(Point point1, Point point2)
        {
            return new Vector(point1.X - point2.X, point1.Y - point2.Y);
        }
    }
}