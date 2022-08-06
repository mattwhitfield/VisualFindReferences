using System;
using System.Windows;

namespace VisualFindReferences.Core.Graph.View
{
    public static class StyleFinder
    {
        public static string ResourceUri { get; set; } = "/VisualFindReferences.Core;component/Themes/generic.xaml";

        public static void Apply(DependencyObject element, string styleName)
        {
            if (element is FrameworkElement frameworkElement)
            {
                var resourceDictionary = new ResourceDictionary
                {
                    Source = new Uri(ResourceUri, UriKind.RelativeOrAbsolute)
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