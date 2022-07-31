using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using VisualFindReferences.Core.Graph.Helper;
using VisualFindReferences.Core.Graph.Model;
using VisualFindReferences.Core.Graph.View;

namespace VisualFindReferences.Core.Graph.ViewModel
{
    public class FlowChartViewModel : ViewModelBase
    {
        public FlowChartView? View { get; set; }

        public FlowChart Model { get; }

        public ObservableCollection<NodeViewModel> NodeViewModels { get; } = new ObservableCollection<NodeViewModel>();

        public ObservableCollection<ConnectorViewModel> ConnectorViewModels { get; } = new ObservableCollection<ConnectorViewModel>();

        public FlowChartViewModel(FlowChart flowChart) : base(flowChart)
        {
            Model = flowChart;
            flowChart.Nodes.CollectionChanged += Nodes_CollectionChanged;
            flowChart.Connectors.CollectionChanged += Connectors_CollectionChanged;
        }

        private void Connectors_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            using (View?.Dispatcher.DisableProcessing())
            {
                HandleCollectionChanged<Connector, ConnectorViewModel>(e, ConnectorViewModels, x => x.ViewModel);
            }
        }

        private void Nodes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            using (View?.Dispatcher.DisableProcessing())
            {
                var removedNodes = HandleCollectionChanged<Node, NodeViewModel>(e, NodeViewModels, x => x.ViewModel);
                foreach (var connector in Model.Connectors.ToList())
                {
                    if (removedNodes.Contains(connector.StartNode) || removedNodes.Contains(connector.EndNode))
                    {
                        Model.Connectors.Remove(connector);
                    }
                }
            }
        }

        private ISet<TModel> HandleCollectionChanged<TModel, TViewModel>(NotifyCollectionChangedEventArgs e, ObservableCollection<TViewModel> targetCollection, Func<TModel, TViewModel> selector)
        {
            var oldItems = new HashSet<TModel>(e.OldItems?.OfType<TModel>() ?? Enumerable.Empty<TModel>());
            var newItems = e.NewItems?.OfType<TModel>() ?? Enumerable.Empty<TModel>();

            foreach (var item in newItems)
            {
                if (oldItems.Remove(item))
                {
                    // was in old items - so is just a change, ignore
                }
                else
                {
                    targetCollection.Add(selector(item));
                }
            }

            // items left in old items are ones that are not in new items
            oldItems.Each(x => targetCollection.Remove(selector(x)));
            return oldItems;
        }
    }
}