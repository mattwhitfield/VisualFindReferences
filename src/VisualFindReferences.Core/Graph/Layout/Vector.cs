using System;

namespace VisualFindReferences.Core.Graph.Layout
{
    public struct Vector : IEquatable<Vector>
    {
        public double X { get; set; }

        public double Y { get; set; }

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Vector vector1, Vector vector2)
        {
            return Equals(vector1, vector2);
        }

        public static bool operator !=(Vector vector1, Vector vector2)
        {
            return !(vector1 == vector2);
        }

        public static bool Equals(Vector vector1, Vector vector2)
        {
            return MathUtils.NearEqual(vector1.X, vector2.X) && MathUtils.NearEqual(vector1.Y, vector2.Y);
        }

        public override bool Equals(object obj)
        {
            return obj is Vector vector && Equals(this, vector);
        }

        public bool Equals(Vector other)
        {
            return Equals(this, other);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public double Length => Math.Sqrt(X * X + Y * Y);

        public static Vector operator +(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X + vector2.X, vector1.Y + vector2.Y);
        }

        public static Vector operator -(Vector vector1, Vector vector2)
        {
            return new Vector(vector1.X - vector2.X, vector1.Y - vector2.Y);
        }

        public static Vector operator *(Vector vector, double scalar)
        {
            return new Vector(vector.X * scalar, vector.Y * scalar);
        }

        public static Vector operator /(Vector vector, double scalar)
        {
            return vector * (1.0 / scalar);
        }
    }
}