using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace VisualFindReferences.Core.Graph.View
{
    public class ColorLerp : MarkupExtension
    {
        public Color FirstColor { get; set; }
        public Color SecondColor { get; set; }
        public double Blend { get; set; }

        public double Opacity { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Colors.Black;
        }
    }
}
