using System;
using System.Windows;

namespace VisualFindReferences.Core.Graph.View
{
    public static class StyleFinder
    {
        public static void Apply(DependencyObject element, string styleName)
        {
            if (element is FrameworkElement frameworkElement)
            {
                var resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri("/VisualFindReferences.Core;component/Themes/generic.xaml", UriKind.RelativeOrAbsolute)
                };

                var style = resourceDictionary[styleName] as Style;
                if (style == null)
                {
                    style = Application.Current.TryFindResource(styleName) as Style;
                }
                frameworkElement.Style = style;
            }
        }
    }
}