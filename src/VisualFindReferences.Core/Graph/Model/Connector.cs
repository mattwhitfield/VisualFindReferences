using VisualFindReferences.Core.Graph.ViewModel;

namespace VisualFindReferences.Core.Graph.Model
{
    public class Connector : ModelBase
    {
        public FlowChart FlowChart { get; }

        public ConnectorViewModel ViewModel { get; }

        public Node StartNode { get; }

        public Node EndNode { get; }

        public Connector(FlowChart flowChart, Node startNode, Node endNode)
        {
            FlowChart = flowChart;
            StartNode = startNode;
            EndNode = endNode;
            ViewModel = new ConnectorViewModel(this);
        }
    }
}