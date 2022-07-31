using System;
using System.Collections.ObjectModel;
using VisualFindReferences.Core.Graph.Model;
using VisualFindReferences.Core.Graph.View;

namespace VisualFindReferences.Core.Graph.ViewModel
{
    public class NodeViewModel : ViewModelBase
    {
        public NodeView? View { get; set; }

        public Node Model { get; }

        public ObservableCollection<Connector> OutboundConnectors { get; } = new ObservableCollection<Connector>();

        private bool _IsSelected;

        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (value != _IsSelected)
                {
                    _IsSelected = value;
                    RaisePropertyChanged("IsSelected");
                }
            }
        }

        public NodeViewModel(Node node) : base(node)
        {
            Model = node ?? throw new ArgumentException("Node can not be null in NodeViewModel constructor");
        }
    }
}