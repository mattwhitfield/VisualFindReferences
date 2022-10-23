﻿using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VisualFindReferences.Core.Graph.Helper;
using VisualFindReferences.Core.Graph.Layout;
using VisualFindReferences.Core.Graph.ViewModel;

namespace VisualFindReferences.Core.Graph.Model
{
    public class NodeGraph : ModelBase
    {
        public NodeGraphViewModel ViewModel { get; }

        public ObservableCollection<Node> Nodes { get; } = new ObservableCollection<Node>();

        public ObservableCollection<Connector> Connectors { get; } = new ObservableCollection<Connector>();

        private Dictionary<Node, List<Connector>> _connectorsByStartNode = new Dictionary<Node, List<Connector>>();

        public NodeGraph()
        {
            ViewModel = CreateViewModel();

            Nodes.CollectionChanged += Nodes_CollectionChanged;
            Connectors.CollectionChanged += Connectors_CollectionChanged;
        }

        protected virtual NodeGraphViewModel CreateViewModel()
        {
            return new NodeGraphViewModel(this);
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

            if (_connectorsByStartNode.TryGetValue(connector.EndNode, out var reverseList))
            {
                foreach (var reversedLink in reverseList.Where(x => x.EndNode == connector.StartNode))
                {
                    reversedLink.ViewModel.IsBidirectional = true;
                    connector.ViewModel.IsBidirectional = true;
                }
            }
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
            TraverseFrom(targetNode, rootNode, stack);
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
                    if (!stack.Any(x => x == connector.ViewModel))
                    {
                        stack.Push(connector.ViewModel);
                        TraverseFrom(connector.EndNode, targetNode, stack);
                        stack.Pop();
                    }
                }
            }

            stack.Pop();
        }

        private void Apply(Stack<IHighlightable> stack)
        {
            stack.Each(x => x.IsHighlighted = true);
        }

        public IDictionary<Node, GraphPoint> GetLayoutPositions(LayoutAlgorithmType algorithmType)
        {
            var nodesToLayout = new Dictionary<Node, GraphPoint>();
            foreach (var node in Nodes)
            {
                nodesToLayout[node] = new GraphPoint(node.X, node.Y);
            }

            return GetLayoutPositions(algorithmType, nodesToLayout);
        }

        public IDictionary<Node, GraphPoint> GetLayoutPositions(LayoutAlgorithmType algorithmType, Dictionary<Node, GraphPoint> nodesToLayout)
        {
            var algo = LayoutAlgorithmFactory.Create(algorithmType, this, nodesToLayout);
            algo.Layout();

            return algo.VerticesPositions;
        }

        public void CalculateContentSize(IDictionary<Node, GraphPoint>? proposedPositions, bool bOnlySelected, out GraphRect rect)
        {
            CalculateContentSize(proposedPositions, bOnlySelected, out var minX, out var maxX, out var minY, out var maxY);
            rect = new GraphRect(minX, minY, maxX - minX, maxY - minY);
        }

        public void CalculateContentSize(IDictionary<Node, GraphPoint>? proposedPositions, bool bOnlySelected, out double minX, out double maxX, out double minY, out double maxY)
        {
            minX = double.MaxValue;
            maxX = double.MinValue;
            minY = double.MaxValue;
            maxY = double.MinValue;

            bool hasNodes = false;
            foreach (var node in Nodes)
            {
                var nodeView = node.ViewModel.View;
                if (bOnlySelected && !node.ViewModel.IsSelected)
                {
                    continue;
                }

                if (proposedPositions == null || !proposedPositions.TryGetValue(node, out var position))
                {
                    position = new GraphPoint(node.X, node.Y);
                }

                minX = Math.Min(position.X, minX);
                maxX = Math.Max(position.X + nodeView?.ActualWidth ?? 0, maxX);
                minY = Math.Min(position.Y, minY);
                maxY = Math.Max(position.Y + nodeView?.ActualHeight ?? 0, maxY);
                hasNodes = true;
            }

            if (!hasNodes)
            {
                minX = maxX = minY = maxY = 0.0;
            }
        }

    }
}