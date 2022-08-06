namespace VisualFindReferences.Core.Graph.Model
{
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
