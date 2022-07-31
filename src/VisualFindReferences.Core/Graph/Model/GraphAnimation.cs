using System;
using System.Collections.Generic;
using System.Windows;
using VisualFindReferences.Core.Graph.Helper;
using VisualFindReferences.Core.Graph.View;

namespace VisualFindReferences.Core.Graph.Model
{
    public class GraphAnimation
    {
        public GraphAnimation(ZoomAndPan target, double targetScale, double targetStartX, double targetStartY)
        {
            Target = target;
            ScaleAnimation = new DoubleValueAnimation(Target.Scale, targetScale);
            StartXAnimation = new DoubleValueAnimation(Target.StartX, targetStartX);
            StartYAnimation = new DoubleValueAnimation(Target.StartY, targetStartY);
        }

        public IList<NodeAnimation> NodeAnimations { get; } = new List<NodeAnimation>();

        public DoubleValueAnimation ScaleAnimation { get; }

        public DoubleValueAnimation StartXAnimation { get; }

        public DoubleValueAnimation StartYAnimation { get; }

        public ZoomAndPan Target { get; }

        public void Apply(double factor)
        {
            factor = Math.Max(Math.Min(factor, 1), 0);
            NodeAnimations.Each(x => x.Apply(factor));
            Target.Scale = ScaleAnimation.Lerp(factor);
            Target.StartX = StartXAnimation.Lerp(factor);
            Target.StartY = StartYAnimation.Lerp(factor);
        }
    }

    public struct NodeAnimation
    {
        public NodeAnimation(Node node, double endX, double endY)
        {
            Node = node ?? throw new ArgumentNullException(nameof(node));
            XAnimation = new DoubleValueAnimation(node.X, endX);
            YAnimation = new DoubleValueAnimation(node.Y, endY);
        }

        public Node Node { get; }

        public DoubleValueAnimation XAnimation { get; }

        public DoubleValueAnimation YAnimation { get; }

        public void Apply(double factor)
        {
            Node.X = XAnimation.Lerp(factor);
            Node.Y = YAnimation.Lerp(factor);
        }
    }

    public struct DoubleValueAnimation
    {
        double From { get; }

        double Diff { get; }

        public DoubleValueAnimation(double from, double to)
        {
            From = from;
            Diff = to - from;
        }

        public double Lerp(double factor)
        {
            return From + Diff * factor;
        }
    }

}
