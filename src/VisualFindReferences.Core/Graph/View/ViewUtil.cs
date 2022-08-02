using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace VisualFindReferences.Core.Graph.View
{
    public class ViewUtil
    {
        public static T? FindFirstParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            if (parent == null)
            {
                return null;
            }

            if (parent is T typedParent)
            {
                return typedParent;
            }

            return FindFirstParent<T>(parent);
        }

        public static void FindChildren<T>(DependencyObject parent, List<T> outChildren) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                {
                    outChildren.Add(typedChild);
                }
                FindChildren(child, outChildren);
            }
        }

        public static T? FindChild<T>(DependencyObject? parent, string? childName = null) where T : DependencyObject
        {
            // Confirm parent and childName are valid.
            if (parent == null)
            {
                return null;
            }

            T? foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T? childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child.
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}