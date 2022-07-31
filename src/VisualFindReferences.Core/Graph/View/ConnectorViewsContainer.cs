using System.Windows;
using System.Windows.Controls;

namespace VisualFindReferences.Core.Graph.View
{
    public class ConnectorViewsContainer : ItemsControl
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            StyleFinder.Apply(element, "DefaultConnectorViewStyle");
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ConnectorView();
        }
    }
}