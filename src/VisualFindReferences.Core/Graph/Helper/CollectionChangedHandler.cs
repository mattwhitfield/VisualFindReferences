using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace VisualFindReferences.Core.Graph.Helper
{
    public static class CollectionChangedHandler
    {
        public static ISet<T> HandleCollectionChanged<T>(NotifyCollectionChangedEventArgs e, Action<T>? added, Action<T>? removed)
        {
            var oldItems = new HashSet<T>(e.OldItems?.OfType<T>() ?? Enumerable.Empty<T>());
            var newItems = e.NewItems?.OfType<T>() ?? Enumerable.Empty<T>();

            foreach (var item in newItems)
            {
                if (!oldItems.Remove(item))
                {
                    added?.Invoke(item);
                }
            }

            // items left in old items are ones that are not in new items
            if (removed != null)
            {
                oldItems.Each(removed);
            }
            return oldItems;
        }
    }
}
