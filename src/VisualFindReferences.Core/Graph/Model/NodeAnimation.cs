using System;

namespace VisualFindReferences.Core.Graph.Model
{
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

}
