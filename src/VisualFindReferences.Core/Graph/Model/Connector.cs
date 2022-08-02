using VisualFindReferences.Core.Graph.ViewModel;

namespace VisualFindReferences.Core.Graph.Model
{
    public class Connector : ModelBase
    {
        public NodeGraph NodeGraph { get; }

        public ConnectorViewModel ViewModel { get; }

        public Node StartNode { get; }

        public Node EndNode { get; }

        public Connector(NodeGraph nodeGraph, Node startNode, Node endNode)
        {
            NodeGraph = nodeGraph;
            StartNode = startNode;
            EndNode = endNode;
            ViewModel = new ConnectorViewModel(this);
        }
    }
}