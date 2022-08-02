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
                    RaisePropertyChanged("IsHighlighted");
                }
            }
        }

        public ConnectorViewModel(Connector connection) : base(connection)
        {
            Model = connection;
        }
    }
}