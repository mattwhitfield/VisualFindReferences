using VisualFindReferences.Core.Graph.Model;
using VisualFindReferences.Core.Graph.View;

namespace VisualFindReferences.Core.Graph.ViewModel
{
    public class ConnectorViewModel : ViewModelBase, IHighlightable
    {
        public ConnectorView? View { get; set; }

        public Connector Model { get; }

        private bool _isHighlighted;

        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set
            {
                if (value != _isHighlighted)
                {
                    _isHighlighted = value;
                    RaisePropertyChanged(nameof(IsHighlighted));
                }
            }
        }

        private bool _isBidirectional;

        public bool IsBidirectional
        {
            get { return _isBidirectional; }
            set
            {
                if (value != _isBidirectional)
                {
                    _isBidirectional = value;
                    RaisePropertyChanged(nameof(IsBidirectional));
                }
            }
        }

        public ConnectorViewModel(Connector connection) : base(connection)
        {
            Model = connection;
        }
    }
}