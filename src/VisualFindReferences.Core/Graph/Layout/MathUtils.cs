using System;

namespace VisualFindReferences.Core.Graph.Layout
{
    public static class MathUtils
    {
        public const double DoubleEpsilon = 2.2204460492503131e-016;

        public static bool IsZero(double a)
        {
            return Math.Abs(a) < 10.0 * DoubleEpsilon;
        }

        public static bool NearEqual(double a, double b)
        {
            return a == b || IsZero(a - b);
        }
    }
}