using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VisualFindReferences.Core.Graph.Helper;
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

        internal void SetHighlightFromRootTo(Node targetNode)
        {
            var rootNode = Nodes.FirstOrDefault(x => x.IsRoot);
            if (rootNode == null)
            {
                return;
            }

            if (rootNode == targetNode)
            {
                return;
            }

            var connectorsByStartNode = new Dictionary<Node, List<Connector>>();
            foreach (var item in Nodes)
            {
                connectorsByStartNode[item] = new List<Connector>();
            }

            foreach (var connector in Connectors)
            {
                connectorsByStartNode[connector.StartNode].Add(connector);
            }

            var stack = new Stack<IHighlightable>();
            TraverseFrom(rootNode, targetNode, stack, connectorsByStartNode);
        }

        private void TraverseFrom(Node startNode, Node targetNode, Stack<IHighlightable> stack, Dictionary<Node, List<Connector>> connectorsByStartNode)
        {
            stack.Push(startNode.ViewModel);
            if (startNode == targetNode)
            {
                Apply(stack);
            }

            foreach (var connector in connectorsByStartNode[startNode])
            {
                stack.Push(connector.ViewModel);
                TraverseFrom(connector.EndNode, targetNode, stack, connectorsByStartNode);
                stack.Pop();
            }

            stack.Pop();
        }

        private void Apply(Stack<IHighlightable> stack)
        {
            stack.Each(x => x.IsHighlighted = true);
        }
    }
}