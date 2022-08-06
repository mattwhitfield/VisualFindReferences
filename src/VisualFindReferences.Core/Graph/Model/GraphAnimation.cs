using System;
using System.Collections.Generic;
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
}
