using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VisualFindReferences.Core.Graph.Helper;
using VisualFindReferences.Core.Graph.Model.Nodes;
using VisualFindReferences.Core.Graph.ViewModel;

namespace VisualFindReferences.Core.Graph.Model
{
    public class VFRNodeGraph : NodeGraph
    {
        private Dictionary<ISymbol, VFRNode> _nodesByTargetSymbol = new Dictionary<ISymbol, VFRNode>(SymbolEqualityComparer.Default);

        protected override void NodeAdded(Node node)
        {
            base.NodeAdded(node);

            if (node is VFRNode vfrNode)
            {
                _nodesByTargetSymbol[vfrNode.NodeFoundReferences.Symbol] = vfrNode;
            }
        }

        protected override void NodeRemoved(Node node)
        {
            base.NodeRemoved(node);

            if (node is VFRNode vfrNode)
            {
                _nodesByTargetSymbol.Remove(vfrNode.NodeFoundReferences.Symbol);
            }
        }

        public bool GetNodeFor(ISymbol symbol, out VFRNode targetNode)
        {
            return _nodesByTargetSymbol.TryGetValue(symbol, out targetNode);
        }
    }

    public class NodeGraph : ModelBase
    {
        public NodeGraphViewModel ViewModel { get; }

        public ObservableCollection<Node> Nodes { get; } = new ObservableCollection<Node>();

        public ObservableCollection<Connector> Connectors { get; } = new ObservableCollection<Connector>();

        private Dictionary<Node, List<Connector>> _connectorsByStartNode = new Dictionary<Node, List<Connector>>();

        public NodeGraph()
        {
            ViewModel = new NodeGraphViewModel(this);

            Nodes.CollectionChanged += Nodes_CollectionChanged;
            Connectors.CollectionChanged += Connectors_CollectionChanged;
        }

        private void Nodes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CollectionChangedHandler.HandleCollectionChanged<Node>(e, NodeAdded, NodeRemoved);
        }
        private void Connectors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CollectionChangedHandler.HandleCollectionChanged<Connector>(e, ConnectorAdded, ConnectorRemoved);
        }

        protected virtual void NodeAdded(Node node) { }

        protected virtual void NodeRemoved(Node node)
        {
            _connectorsByStartNode.Remove(node);
        }

        protected virtual void ConnectorAdded(Connector connector)
        {
            if (!_connectorsByStartNode.TryGetValue(connector.StartNode, out var list))
            {
                _connectorsByStartNode[connector.StartNode] = list = new List<Connector>();
            }
            list.Add(connector);
        }

        protected virtual void ConnectorRemoved(Connector connector)
        {
            if (_connectorsByStartNode.TryGetValue(connector.StartNode, out var list))
            {
                list.Remove(connector);
            }
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

            var stack = new Stack<IHighlightable>();
            TraverseFrom(rootNode, targetNode, stack);
        }

        private void TraverseFrom(Node startNode, Node targetNode, Stack<IHighlightable> stack)
        {
            stack.Push(startNode.ViewModel);
            if (startNode == targetNode)
            {
                Apply(stack);
            }

            if (_connectorsByStartNode.TryGetValue(startNode, out var list))
            {
                foreach (var connector in list)
                {
                    stack.Push(connector.ViewModel);
                    TraverseFrom(connector.EndNode, targetNode, stack);
                    stack.Pop();
                }
            }

            stack.Pop();
        }

        private void Apply(Stack<IHighlightable> stack)
        {
            stack.Each(x => x.IsHighlighted = true);
        }
    }
}