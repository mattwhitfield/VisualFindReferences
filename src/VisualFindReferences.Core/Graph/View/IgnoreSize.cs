using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VisualFindReferences.Core.Graph.View
{
    public class IgnoreSize : Decorator
    {
        protected override Size MeasureOverride(Size constraint)
        {
            Child.Measure(new Size(200, 20));
            return new Size(0, 0);
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Child.Measure(new Size(200, 20));
            Child.Arrange(new Rect(Child.DesiredSize));
            return Child.DesiredSize;
        }

        protected override Geometry GetLayoutClip(Size layoutSlotSize)
        {
#pragma warning disable CS8603 // Possible null reference return - deliberate - we don't want clipping, and the base library nullability is declared incorrectly
            return null;
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
}
