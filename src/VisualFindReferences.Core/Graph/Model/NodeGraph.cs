using System.Collections.ObjectModel;
using VisualFindReferences.Core.Graph.ViewModel;

namespace VisualFindReferences.Core.Graph.Model
{
    public class NodeGraph : ModelBase
    {
        public NodeGraphViewModel ViewModel { get; }

        public ObservableCollection<Node> Nodes { get; } = new ObservableCollection<Node>();

        public ObservableCollection<Connector> Connectors { get; } = new ObservableCollection<Connector>();

        public NodeGraph()
        {
            ViewModel = new NodeGraphViewModel(this);
        }
    }
}