using System;
using System.Collections.Generic;
using System.Linq;
using VisualFindReferences.Core.Graph.Model;

namespace VisualFindReferences.Core.Graph.Layout
{
    public class FSAAlgorithm
    {
        private class RectangleWrapper<TObject>
        {
            public TObject Id { get; }

            public GraphRect Rectangle;

            public RectangleWrapper(TObject id, GraphRect rectangle)
            {
                Id = id;
                Rectangle = rectangle;
            }

            public double CenterX => Rectangle.Left + Rectangle.Width / 2;

            public double CenterY => Rectangle.Top + Rectangle.Height / 2;
        }

        private List<RectangleWrapper<Node>> WrappedRectangles;

        public IDictionary<Node, GraphRect> Rectangles { get; }

        public FSAAlgorithm(IDictionary<Node, GraphRect> rectangles)
        {
            // Original rectangles
            Rectangles = rectangles ?? throw new ArgumentNullException(nameof(rectangles));

            // Wrapping the old rectangles, to remember which one belongs to which object
            WrappedRectangles = rectangles.Select(x => new RectangleWrapper<Node>(x.Key, x.Value)).ToList();
        }

        public void InternalCompute()
        {
            if (WrappedRectangles.Count == 0)
                return;

            AddGaps();

            HorizontalImproved();
            VerticalImproved();

            RemoveGaps();

            foreach (RectangleWrapper<Node> rectangle in WrappedRectangles)
            {
                Rectangles[rectangle.Id] = rectangle.Rectangle;
            }
        }

        protected virtual void AddGaps()
        {
            foreach (RectangleWrapper<Node> wrapper in WrappedRectangles)
            {
                wrapper.Rectangle.Width += ParametersHorizontalGap;
                wrapper.Rectangle.Height += ParametersVerticalGap;
                wrapper.Rectangle.Offset(-ParametersHorizontalGap / 2, -ParametersVerticalGap / 2);
            }
        }

        private const double ParametersHorizontalGap = 10;
        private const double ParametersVerticalGap = 10;

        protected virtual void RemoveGaps()
        {
            foreach (RectangleWrapper<Node> wrapper in WrappedRectangles)
            {
                wrapper.Rectangle.Width -= ParametersHorizontalGap;
                wrapper.Rectangle.Height -= ParametersVerticalGap;
                wrapper.Rectangle.Offset(ParametersHorizontalGap / 2, ParametersVerticalGap / 2);
            }
        }

        protected GraphVector Force(GraphRect vi, GraphRect vj)
        {
            var force = new GraphVector(0, 0);
            GraphVector distance = vj.GetCenter() - vi.GetCenter();
            double absDistanceX = Math.Abs(distance.X);
            double absDistanceY = Math.Abs(distance.Y);
            double gij = distance.Y / distance.X;
                        double Gij = (vi.Height + vj.Height) / (vi.Width + vj.Width);

            if (Gij >= gij && gij > 0 || -Gij <= gij && gij < 0 || MathUtils.IsZero(gij))
            {
                // vi and vj touch with y-direction boundaries
                force.X = distance.X / absDistanceX * ((vi.Width + vj.Width) / 2.0 - absDistanceX);
                force.Y = force.X * gij;
            }

            if (Gij < gij && gij > 0 || -Gij > gij && gij < 0)
            {
                // vi and vj touch with x-direction boundaries
                force.Y = distance.Y / absDistanceY * ((vi.Height + vj.Height) / 2.0 - absDistanceY);
                force.X = force.Y / gij;
            }

            return force;
        }

        protected GraphVector Force2(GraphRect vi, GraphRect vj)
        {
            var force = new GraphVector(0, 0);
            GraphVector distance = vj.GetCenter() - vi.GetCenter();
            double gij = distance.Y / distance.X;
            if (vi.IntersectsWith(vj))
            {
                force.X = (vi.Width + vj.Width) / 2.0 - distance.X;
                force.Y = (vi.Height + vj.Height) / 2.0 - distance.Y;
                // In the X dimension
                if (force.X > force.Y && !MathUtils.IsZero(gij))
                {
                    force.X = force.Y / gij;
                }

                force.X = Math.Max(force.X, 0);
                force.Y = Math.Max(force.Y, 0);
            }

            return force;
        }

        private int XComparison(RectangleWrapper<Node> r1, RectangleWrapper<Node> r2)
        {
            double r1CenterX = r1.CenterX;
            double r2CenterX = r2.CenterX;

            if (r1CenterX < r2CenterX)
                return -1;
            if (r1CenterX > r2CenterX)
                return 1;
            return 0;
        }

        private double HorizontalImproved()
        {
            WrappedRectangles.Sort(XComparison);
            int i = 0;
            int n = WrappedRectangles.Count;

            // Left side
            RectangleWrapper<Node> leftMin = WrappedRectangles[0];
            double sigma = 0;
            double x0 = leftMin.CenterX;
            var gamma = new double[WrappedRectangles.Count];
            var x = new double[WrappedRectangles.Count];
            while (i < n)
            {
                RectangleWrapper<Node> u = WrappedRectangles[i];

                // Rectangle with the same center than Rectangle[i]
                int k = i;
                for (int j = i + 1; j < n; ++j)
                {
                    RectangleWrapper<Node> v = WrappedRectangles[j];
                    if (MathUtils.NearEqual(u.CenterX, v.CenterX))
                    {
                        u = v;
                        k = j;
                    }
                    else
                    {
                        break;
                    }
                }

                double g = 0;
                // For rectangles in [i, k], compute the left force
                if (u.CenterX > x0)
                {
                    for (int m = i; m <= k; ++m)
                    {
                        double ggg = 0;
                        for (int j = 0; j < i; ++j)
                        {
                            GraphVector force = Force(WrappedRectangles[j].Rectangle, WrappedRectangles[m].Rectangle);
                            ggg = Math.Max(force.X + gamma[j], ggg);
                        }

                        RectangleWrapper<Node> v = WrappedRectangles[m];
                        double gg = v.Rectangle.Left + ggg < leftMin.Rectangle.Left
                            ? sigma
                            : ggg;
                        g = Math.Max(g, gg);
                    }
                }

                // Compute offset to elements in x
                // and redefine left side
                for (int m = i; m <= k; ++m)
                {
                    gamma[m] = g;
                    RectangleWrapper<Node> r = WrappedRectangles[m];
                    x[m] = r.Rectangle.Left + g;
                    if (r.Rectangle.Left < leftMin.Rectangle.Left)
                    {
                        leftMin = r;
                    }
                }

                // Compute the right force of rectangles in [i, k] and store the maximal one
                // delta = max(0, max{f.x(m,j)|i<=m<=k<j<n})
                double delta = 0;
                for (int m = i; m <= k; ++m)
                {
                    for (int j = k + 1; j < n; ++j)
                    {
                        GraphVector force = Force(WrappedRectangles[m].Rectangle, WrappedRectangles[j].Rectangle);
                        if (force.X > delta)
                        {
                            delta = force.X;
                        }
                    }
                }

                sigma += delta;
                i = k + 1;
            }

            double cost = 0;
            for (i = 0; i < n; ++i)
            {
                RectangleWrapper<Node> r = WrappedRectangles[i];
                double oldPos = r.Rectangle.Left;
                double newPos = x[i];

                r.Rectangle.X = newPos;

                double diff = oldPos - newPos;
                cost += diff * diff;
            }

            return cost;
        }

        private int YComparison(
            RectangleWrapper<Node> r1,
            RectangleWrapper<Node> r2)
        {
            double r1CenterY = r1.CenterY;
            double r2CenterY = r2.CenterY;

            if (r1CenterY < r2CenterY)
                return -1;
            if (r1CenterY > r2CenterY)
                return 1;
            return 0;
        }

        private double VerticalImproved()
        {
            WrappedRectangles.Sort(YComparison);
            int i = 0;
            int n = WrappedRectangles.Count;

            RectangleWrapper<Node> topMin = WrappedRectangles[0];
            double sigma = 0;
            double y0 = topMin.CenterY;
            var gamma = new double[WrappedRectangles.Count];
            var y = new double[WrappedRectangles.Count];
            while (i < n)
            {
                RectangleWrapper<Node> u = WrappedRectangles[i];

                int k = i;
                for (int j = i + 1; j < n; ++j)
                {
                    RectangleWrapper<Node> v = WrappedRectangles[j];
                    if (MathUtils.NearEqual(u.CenterY, v.CenterY))
                    {
                        u = v;
                        k = j;
                    }
                    else
                    {
                        break;
                    }
                }

                double g = 0;
                if (u.CenterY > y0)
                {
                    for (int m = i; m <= k; ++m)
                    {
                        double ggg = 0;
                        for (int j = 0; j < i; ++j)
                        {
                            GraphVector f = Force2(WrappedRectangles[j].Rectangle, WrappedRectangles[m].Rectangle);
                            ggg = Math.Max(f.Y + gamma[j], ggg);
                        }

                        RectangleWrapper<Node> v = WrappedRectangles[m];
                        double gg = v.Rectangle.Top + ggg < topMin.Rectangle.Top
                            ? sigma
                            : ggg;
                        g = Math.Max(g, gg);
                    }
                }

                for (int m = i; m <= k; ++m)
                {
                    gamma[m] = g;
                    RectangleWrapper<Node> r = WrappedRectangles[m];
                    y[m] = r.Rectangle.Top + g;
                    if (r.Rectangle.Top < topMin.Rectangle.Top)
                    {
                        topMin = r;
                    }
                }

                // delta = max(0, max{f.x(m,j)|i<=m<=k<j<n})
                double delta = 0;
                for (int m = i; m <= k; ++m)
                {
                    for (int j = k + 1; j < n; ++j)
                    {
                        GraphVector force = Force(WrappedRectangles[m].Rectangle, WrappedRectangles[j].Rectangle);
                        if (force.Y > delta)
                        {
                            delta = force.Y;
                        }
                    }
                }
                sigma += delta;
                i = k + 1;
            }

            double cost = 0;
            for (i = 0; i < n; ++i)
            {
                RectangleWrapper<Node> r = WrappedRectangles[i];
                double oldPos = r.Rectangle.Top;
                double newPos = y[i];

                r.Rectangle.Y = newPos;

                double diff = oldPos - newPos;
                cost += diff * diff;
            }

            return cost;
        }
    }
}