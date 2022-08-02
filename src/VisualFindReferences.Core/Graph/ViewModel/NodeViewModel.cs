using System;
using System.Collections.ObjectModel;
using VisualFindReferences.Core.Graph.Model;
using VisualFindReferences.Core.Graph.View;

namespace VisualFindReferences.Core.Graph.ViewModel
{
    public class NodeViewModel : ViewModelBase, IHighlightable
    {
        public NodeView? View { get; set; }

        public Node Model { get; }

        public ObservableCollection<Connector> OutboundConnectors { get; } = new ObservableCollection<Connector>();


        private bool _isSelected;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    RaisePropertyChanged("IsSelected");
                }
            }
        }

        private bool _isHighlighted;

        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set
            {
                if (value != _isHighlighted)
                {
                    _isHighlighted = value;
                    RaisePropertyChanged("IsHighlighted");
                }
            }
        }

        public NodeViewModel(Node node) : base(node)
        {
            Model = node ?? throw new ArgumentException("Node can not be null in NodeViewModel constructor");
        }
    }
}