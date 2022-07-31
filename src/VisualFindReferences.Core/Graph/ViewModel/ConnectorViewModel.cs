using VisualFindReferences.Core.Graph.Model;
using VisualFindReferences.Core.Graph.View;

namespace VisualFindReferences.Core.Graph.ViewModel
{
    public class ConnectorViewModel : ViewModelBase
    {
        public ConnectorView? View { get; set; }

        public Connector Model { get; }

        public ConnectorViewModel(Connector connection) : base(connection)
        {
            Model = connection;
        }
    }
}