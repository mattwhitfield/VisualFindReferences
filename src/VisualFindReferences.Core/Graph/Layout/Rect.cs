﻿using System;

namespace VisualFindReferences.Core.Graph.Layout
{
    public struct Rect : IEquatable<Rect>
    {
        public Point GetCenter()
        {
            return new Point(
                Left + Width / 2,
                Top + Height / 2);
        }

        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public Rect(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public static bool operator ==(Rect rect1, Rect rect2)
        {
            return Equals(rect1, rect2);
        }

        public static bool operator !=(Rect rect1, Rect rect2)
        {
            return !(rect1 == rect2);
        }

        public static bool Equals(Rect rect1, Rect rect2)
        {
            return MathUtils.NearEqual(rect1.X, rect2.X)
                   && MathUtils.NearEqual(rect1.Y, rect2.Y)
                   && MathUtils.NearEqual(rect1.Width, rect2.Width)
                   && MathUtils.NearEqual(rect1.Height, rect2.Height);
        }

        public override bool Equals(object obj)
        {
            return obj is Rect rect && Equals(this, rect);
        }

        public bool Equals(Rect other)
        {
            return Equals(this, other);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
        }

        public double Left => X;

        public double Top => Y;

        public double Right => X + Width;

        public double Bottom => Y + Height;

        public bool IntersectsWith(Rect rect)
        {
            return rect.Left <= Right && rect.Right >= Left && rect.Top <= Bottom && rect.Bottom >= Top;
        }

        public void Offset(double offsetX, double offsetY)
        {
            X += offsetX;
            Y += offsetY;
        }
    }
}