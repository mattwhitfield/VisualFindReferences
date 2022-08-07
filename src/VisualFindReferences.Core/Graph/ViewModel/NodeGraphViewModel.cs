using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VisualFindReferences.Core.Graph.Helper;
using VisualFindReferences.Core.Graph.Layout;
using VisualFindReferences.Core.Graph.Model;
using VisualFindReferences.Core.Graph.View;

namespace VisualFindReferences.Core.Graph.ViewModel
{
    public class NodeGraphViewModel : ViewModelBase
    {
        public NodeGraphView? View { get; set; }

        public NodeGraph Model { get; }

        public ObservableCollection<NodeViewModel> NodeViewModels { get; } = new ObservableCollection<NodeViewModel>();

        public ObservableCollection<ConnectorViewModel> ConnectorViewModels { get; } = new ObservableCollection<ConnectorViewModel>();

        public NodeGraphViewModel(NodeGraph nodeGraph) : base(nodeGraph)
        {
            Model = nodeGraph;
            nodeGraph.Nodes.CollectionChanged += Nodes_CollectionChanged;
            nodeGraph.Connectors.CollectionChanged += Connectors_CollectionChanged;
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

        public void RunAction<T>(Func<Action<string>, NodeGraphViewModel, Task<T>> task, Action<T, NodeGraph> continuation)
        {
            IsBusy = true;
            BusyText = "Loading...";

            Task.Factory.StartNew(async () =>
            {
                try
                {
                    var result = await task(SetBusyText, this);
                    View?.Dispatcher.Invoke(new Action(() =>
                    {
                        IsBusy = false;
                        continuation(result, Model);
                    }));
                }
                catch (Exception e)
                {
                    View?.Dispatcher.Invoke(new Action(() =>
                    {
                        IsBusy = false;
                        MessageBox.Show("Error occurred while executing operation: " + e.Message, "Error occurred", MessageBoxButton.OK, MessageBoxImage.Error);
                    }));
                }
            });
        }

        public void ApplyLayout(bool fitToDisplay)
        {
            if (View != null)
            {
                var positions = Model.GetLayoutPositions(LayoutType);

                var proposedZoomAndPan = View.ZoomAndPan;

                if (fitToDisplay)
                {
                    Model.CalculateContentSize(positions, false, out var rect);
                    proposedZoomAndPan = View.ZoomAndPan.GetTarget(rect);
                }

                View.StartAnimation(positions, proposedZoomAndPan.Scale, proposedZoomAndPan.StartX, proposedZoomAndPan.StartY);
            }
        }

        private void SetBusyText(string text)
        {
            View?.Dispatcher.Invoke(new Action(() => BusyText = text));
        }

        private LayoutAlgorithmType _layoutType = LayoutAlgorithmType.VerticalBalancedGrid;

        public LayoutAlgorithmType LayoutType
        {
            get { return _layoutType; }
            set
            {
                if (value != _layoutType)
                {
                    _layoutType = value;
                    RaisePropertyChanged(nameof(LayoutType));
                }
            }
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value != _isBusy)
                {
                    _isBusy = value;
                    RaisePropertyChanged(nameof(IsBusy));
                }
            }
        }

        private string _busyText = string.Empty;

        public string BusyText
        {
            get { return _busyText; }
            set
            {
                if (value != _busyText)
                {
                    _busyText = value;
                    RaisePropertyChanged(nameof(BusyText));
                }
            }
        }

        private ISet<TModel> HandleCollectionChanged<TModel, TViewModel>(NotifyCollectionChangedEventArgs e, ObservableCollection<TViewModel> targetCollection, Func<TModel, TViewModel> selector)
        {
            return CollectionChangedHandler.HandleCollectionChanged<TModel>(e, item => targetCollection.Add(selector(item)), item => targetCollection.Remove(selector(item)));
        }
    }
}